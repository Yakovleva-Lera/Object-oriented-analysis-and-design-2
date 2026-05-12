package com.expensetracker.gateway;

import com.expensetracker.model.Expense;
import java.util.List;

public interface ExpenseGateway {
    void save(Expense expense);
    List<Expense> findAll();
    List<Expense> findByCategory(String category);
    List<Expense> findAllSorted(String sortColumn);
    void delete(int id);
    
    // Новые методы
    double getSumByRange(String startDate, String endDate);
    void resetDatabase();
}