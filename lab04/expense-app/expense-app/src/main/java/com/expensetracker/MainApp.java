package com.expensetracker;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;
import com.expensetracker.controller.ExpenseController;
import com.expensetracker.gateway.ExpenseGateway;
import com.expensetracker.gateway.SQLiteExpenseGateway;
import java.io.IOException;

public class MainApp extends Application {
    @Override
    public void start(Stage primaryStage) throws IOException {
        // Загружаем интерфейс из fxml файла
        FXMLLoader loader = new FXMLLoader(getClass().getResource("/fxml/expense.fxml"));
        Parent root = loader.load();

        // 1. Создаем шлюз (Gateway) - здесь происходит магия подключения к БД
        ExpenseGateway gateway = new SQLiteExpenseGateway();

        // 2. Получаем контроллер и передаем ему шлюз (Dependency Injection)
        ExpenseController controller = loader.getController();
        controller.setGateway(gateway);

        primaryStage.setTitle("Учёт расходов (MVP)");
        primaryStage.setScene(new Scene(root, 650, 450));
        primaryStage.show();
    }

    public static void main(String[] args) {
        launch(args);
    }
}