package com.expensetracker.gateway;

import com.expensetracker.model.Expense;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class SQLiteExpenseGateway implements ExpenseGateway {
    private Connection connection;

    public SQLiteExpenseGateway() {
        try {
            connection = DriverManager.getConnection("jdbc:sqlite:expenses.db");
            createTableIfNotExists();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void createTableIfNotExists() throws SQLException {
        String sql = "CREATE TABLE IF NOT EXISTS expenses (" +
                     "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                     "amount REAL NOT NULL, " +
                     "category TEXT NOT NULL, " +
                     "date TEXT NOT NULL, " +
                     "comment TEXT)";
        try (Statement stmt = connection.createStatement()) {
            stmt.execute(sql);
        }
    }

    @Override
    public void save(Expense expense) {
        String sql = "INSERT INTO expenses(amount, category, date, comment) VALUES(?, ?, ?, ?)";
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setDouble(1, expense.getAmount());
            pstmt.setString(2, expense.getCategory());
            pstmt.setString(3, expense.getDate());
            pstmt.setString(4, expense.getComment());
            pstmt.executeUpdate();
        } catch (SQLException e) { e.printStackTrace(); }
    }

    @Override
    public List<Expense> findAll() {
        return query("SELECT * FROM expenses");
    }

    @Override
    public List<Expense> findByCategory(String category) {
        return query("SELECT * FROM expenses WHERE category = ?", category);
    }

    @Override
    public List<Expense> findAllSorted(String sortColumn) {
        String column = sortColumn.equals("amount") ? "amount" : "date";
        return query("SELECT * FROM expenses ORDER BY " + column + " DESC");
    }

    private List<Expense> query(String sql, Object... params) {
        List<Expense> list = new ArrayList<>();
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            for (int i = 0; i < params.length; i++) {
                pstmt.setObject(i + 1, params[i]);
            }
            try (ResultSet rs = pstmt.executeQuery()) {
                while (rs.next()) {
                    list.add(new Expense(
                        rs.getInt("id"),
                        rs.getDouble("amount"),
                        rs.getString("category"),
                        rs.getString("date"),
                        rs.getString("comment")
                    ));
                }
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return list;
    }

    @Override
    public void delete(int id) {
        String sql = "DELETE FROM expenses WHERE id = ?";
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setInt(1, id);
            pstmt.executeUpdate();
        } catch (SQLException e) { e.printStackTrace(); }
    }

    @Override
    public double getSumByRange(String startDate, String endDate) {
        String sql = "SELECT COALESCE(SUM(amount), 0) FROM expenses WHERE date BETWEEN ? AND ?";
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, startDate);
            pstmt.setString(2, endDate);
            try (ResultSet rs = pstmt.executeQuery()) {
                if (rs.next()) return rs.getDouble(1);
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return 0.0;
    }

    @Override
    public void resetDatabase() {
        try (Statement stmt = connection.createStatement()) {
            stmt.execute("DELETE FROM expenses");
            stmt.execute("DELETE FROM sqlite_sequence WHERE name='expenses'");
        } catch (SQLException e) { e.printStackTrace(); }
    }
}