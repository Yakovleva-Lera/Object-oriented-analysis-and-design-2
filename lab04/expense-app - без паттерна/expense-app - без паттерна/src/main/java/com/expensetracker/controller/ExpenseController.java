package com.expensetracker.controller;

import com.expensetracker.model.Expense;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import java.sql.*;
import java.time.LocalDate;

public class ExpenseController {
    @FXML private TableView<Expense> expenseTable;
    @FXML private TableColumn<Expense, Integer> colId;
    @FXML private TableColumn<Expense, Double> colAmount;
    @FXML private TableColumn<Expense, String> colCategory;
    @FXML private TableColumn<Expense, String> colDate;
    @FXML private TableColumn<Expense, String> colComment;
    @FXML private TextField txtAmount;
    @FXML private ComboBox<String> comboCategory;
    @FXML private ComboBox<String> comboSort;
    @FXML private DatePicker datePicker;
    @FXML private TextField txtComment;
    @FXML private Label lblTotal;
    @FXML private DatePicker dpStartDate;
    @FXML private DatePicker dpEndDate;

    private Connection connection;
    private ObservableList<Expense> expenseList = FXCollections.observableArrayList();

    @FXML public void initialize() {
        setupTable();
        setupControls();
        initDatabase();
        loadExpenses();
    }

    private void setupTable() {
        colId.setCellValueFactory(c -> new SimpleIntegerProperty(c.getValue().getId()).asObject());
        colAmount.setCellValueFactory(c -> new SimpleDoubleProperty(c.getValue().getAmount()).asObject());
        colCategory.setCellValueFactory(c -> new SimpleStringProperty(c.getValue().getCategory()));
        colDate.setCellValueFactory(c -> new SimpleStringProperty(c.getValue().getDate()));
        colComment.setCellValueFactory(c -> new SimpleStringProperty(c.getValue().getComment()));
        expenseTable.setItems(expenseList);
    }

    private void setupControls() {
        comboCategory.getItems().addAll("Еда", "Транспорт", "Развлечения", "Здоровье", "Другое");
        comboCategory.setValue("Еда");
        comboSort.getItems().addAll("По дате (новые)", "По сумме (большие)");
        comboSort.setValue("По дате (новые)");
        datePicker.setValue(LocalDate.now());
        dpStartDate.setValue(LocalDate.now());
        dpEndDate.setValue(LocalDate.now());
    }

    private void initDatabase() {
        try {
            connection = DriverManager.getConnection("jdbc:sqlite:expenses.db");
            String sql = "CREATE TABLE IF NOT EXISTS expenses (id INTEGER PRIMARY KEY AUTOINCREMENT, amount REAL, category TEXT, date TEXT, comment TEXT)";
            connection.createStatement().execute(sql);
        } catch (SQLException e) { showError("Ошибка подключения к БД"); }
    }

    @FXML private void handleAdd() {
        try {
            double amount = Double.parseDouble(txtAmount.getText());
            String category = comboCategory.getValue();
            String date = datePicker.getValue().toString();
            String comment = txtComment.getText();

            String sql = "INSERT INTO expenses(amount, category, date, comment) VALUES(?, ?, ?, ?)";
            try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
                pstmt.setDouble(1, amount); pstmt.setString(2, category);
                pstmt.setString(3, date); pstmt.setString(4, comment);
                pstmt.executeUpdate();
            }
            clearFields(); loadExpenses();
        } catch (SQLException | NumberFormatException e) { showError("Ошибка добавления"); }
    }

    private void loadExpenses() {
        String sql = "SELECT * FROM expenses";
        expenseList.clear();
        try (Statement stmt = connection.createStatement(); ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) expenseList.add(new Expense(rs.getInt("id"), rs.getDouble("amount"),
                rs.getString("category"), rs.getString("date"), rs.getString("comment")));
        } catch (SQLException e) { showError("Ошибка загрузки"); }
    }

    @FXML private void handleDelete() {
        Expense sel = expenseTable.getSelectionModel().getSelectedItem();
        if (sel != null) {
            try {
                String sql = "DELETE FROM expenses WHERE id = ?";
                try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
                    pstmt.setInt(1, sel.getId()); pstmt.executeUpdate();
                }
                loadExpenses();
            } catch (SQLException e) { showError("Ошибка удаления"); }
        }
    }

    @FXML private void handleSort() {
        String col = comboSort.getValue().contains("сумме") ? "amount" : "date";
        String sql = "SELECT * FROM expenses ORDER BY " + col + " DESC";
        expenseList.clear();
        try (Statement stmt = connection.createStatement(); ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) expenseList.add(new Expense(rs.getInt("id"), rs.getDouble("amount"),
                rs.getString("category"), rs.getString("date"), rs.getString("comment")));
        } catch (SQLException e) { showError("Ошибка сортировки"); }
    }

    @FXML private void handleFilter() {
        String cat = comboCategory.getValue();
        String sql = "SELECT * FROM expenses WHERE category = ?";
        expenseList.clear();
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, cat);
            try (ResultSet rs = pstmt.executeQuery(sql)) {
                while (rs.next()) expenseList.add(new Expense(rs.getInt("id"), rs.getDouble("amount"),
                    rs.getString("category"), rs.getString("date"), rs.getString("comment")));
            }
        } catch (SQLException e) { showError("Ошибка фильтрации"); }
    }

    @FXML private void handleShowAll() { loadExpenses(); }

    @FXML private void calcPeriod() {
        if (dpStartDate.getValue() == null || dpEndDate.getValue() == null) { showError("Выберите даты"); return; }
        String start = dpStartDate.getValue().toString();
        String end = dpEndDate.getValue().toString();
        if (start.compareTo(end) > 0) { showError("Начало > Конец"); return; }

        String sql = "SELECT COALESCE(SUM(amount), 0) FROM expenses WHERE date BETWEEN ? AND ?";
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, start); pstmt.setString(2, end);
            try (ResultSet rs = pstmt.executeQuery()) { if (rs.next()) lblTotal.setText(rs.getDouble(1) + " руб"); }
        } catch (SQLException e) { showError("Ошибка расчёта"); }
    }

    @FXML private void handleReset() {
        Alert a = new Alert(Alert.AlertType.CONFIRMATION, "Удалить всё и сбросить ID?", ButtonType.YES, ButtonType.NO);
        a.showAndWait().ifPresent(r -> {
            if (r == ButtonType.YES) {
                try (Statement stmt = connection.createStatement()) {
                    stmt.execute("DELETE FROM expenses");
                    stmt.execute("DELETE FROM sqlite_sequence WHERE name='expenses'");
                } catch (SQLException e) { showError("Ошибка сброса"); }
                loadExpenses(); lblTotal.setText("0.00 руб");
            }
        });
    }

    private void clearFields() { txtAmount.clear(); txtComment.clear(); datePicker.setValue(LocalDate.now()); }
    private void showError(String msg) { new Alert(Alert.AlertType.ERROR, msg, ButtonType.OK).showAndWait(); }
}