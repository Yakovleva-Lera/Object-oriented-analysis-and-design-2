package com.expensetracker;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;
import java.io.IOException;

public class MainApp extends Application {
    @Override
    public void start(Stage primaryStage) throws IOException {
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/fxml/expense.fxml"));
        Parent root = loader.load();

        primaryStage.setTitle("Учёт расходов (БЕЗ ПАТТЕРНА)");
        primaryStage.setScene(new Scene(root, 650, 450));
        primaryStage.show();
    }

    public static void main(String[] args) { launch(args); }
}