package com.expensetracker.controller;

import com.expensetracker.gateway.ExpenseGateway;
import com.expensetracker.model.Expense;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;

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

    private ExpenseGateway gateway;
    private ObservableList<Expense> expenseList = FXCollections.observableArrayList();

    public void setGateway(ExpenseGateway gateway) {
        this.gateway = gateway;
        loadExpenses();
    }

    @FXML
    public void initialize() {
        colId.setCellValueFactory(cell -> new SimpleIntegerProperty(cell.getValue().getId()).asObject());
        colAmount.setCellValueFactory(cell -> new SimpleDoubleProperty(cell.getValue().getAmount()).asObject());
        colCategory.setCellValueFactory(cell -> new SimpleStringProperty(cell.getValue().getCategory()));
        colDate.setCellValueFactory(cell -> new SimpleStringProperty(cell.getValue().getDate()));
        colComment.setCellValueFactory(cell -> new SimpleStringProperty(cell.getValue().getComment()));

        expenseTable.setItems(expenseList);
        comboCategory.getItems().addAll("Еда", "Транспорт", "Развлечения", "Здоровье", "Другое");
        comboCategory.setValue("Еда");
        datePicker.setValue(LocalDate.now());
        
        comboSort.getItems().addAll("По дате (новые)", "По сумме (большие)");
        comboSort.setValue("По дате (новые)");
        
        // Устанавливаем даты по умолчанию: сегодня
        dpStartDate.setValue(LocalDate.now());
        dpEndDate.setValue(LocalDate.now());
    }

    @FXML private void handleAdd() {
        try {
            double amount = Double.parseDouble(txtAmount.getText());
            String category = comboCategory.getValue();
            String date = datePicker.getValue().format(DateTimeFormatter.ISO_LOCAL_DATE);
            String comment = txtComment.getText();

            gateway.save(new Expense(0, amount, category, date, comment));
            clearFields();
            loadExpenses();
        } catch (NumberFormatException e) {
            showError("Сумма должна быть числом");
        }
    }

    @FXML private void handleDelete() {
        Expense selected = expenseTable.getSelectionModel().getSelectedItem();
        if (selected != null) {
            gateway.delete(selected.getId());
            loadExpenses();
        }
    }

    @FXML private void handleSort() {
        String sortOption = comboSort.getValue();
        String col = (sortOption != null && sortOption.contains("сумме")) ? "amount" : "date";
        expenseList.setAll(gateway.findAllSorted(col));
    }

    @FXML private void handleFilter() {
        String category = comboCategory.getValue();
        if (category != null) expenseList.setAll(gateway.findByCategory(category));
    }

    @FXML private void handleShowAll() {
        loadExpenses();
    }

    @FXML private void calcPeriod() {
        if (dpStartDate.getValue() == null || dpEndDate.getValue() == null) {
            showError("Выберите обе даты");
            return;
        }
        
        String start = dpStartDate.getValue().toString();
        String end = dpEndDate.getValue().toString();
        
        if (start.compareTo(end) > 0) {
            showError("Дата начала не может быть позже даты конца");
            return;
        }
        
        double sum = gateway.getSumByRange(start, end);
        lblTotal.setText(String.format("%.2f руб", sum));
    }

    @FXML private void handleReset() {
        Alert alert = new Alert(Alert.AlertType.CONFIRMATION, 
            "Удалить все записи и сбросить ID к 1?", ButtonType.YES, ButtonType.NO);
        alert.showAndWait().ifPresent(response -> {
            if (response == ButtonType.YES) {
                gateway.resetDatabase();
                loadExpenses();
                lblTotal.setText("0.00 руб");
            }
        });
    }

    private void loadExpenses() { expenseList.setAll(gateway.findAll()); }
    private void clearFields() { txtAmount.clear(); txtComment.clear(); datePicker.setValue(LocalDate.now()); }
    private void showError(String msg) { new Alert(Alert.AlertType.ERROR, msg, ButtonType.OK).showAndWait(); }
}