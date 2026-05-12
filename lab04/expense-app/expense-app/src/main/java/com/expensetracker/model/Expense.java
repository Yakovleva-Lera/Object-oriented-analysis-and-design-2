package com.expensetracker.model;

public class Expense {
    private int id;
    private double amount;
    private String category;
    private String date;
    private String comment;

    public Expense(int id, double amount, String category, String date, String comment) {
        this.id = id;
        this.amount = amount;
        this.category = category;
        this.date = date;
        this.comment = comment;
    }

    public int getId() { return id; }
    public double getAmount() { return amount; }
    public void setAmount(double amount) { this.amount = amount; }
    public String getCategory() { return category; }
    public void setCategory(String category) { this.category = category; }
    public String getDate() { return date; }
    public void setDate(String date) { this.date = date; }
    public String getComment() { return comment; }
    public void setComment(String comment) { this.comment = comment; }
}