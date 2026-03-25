#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <map>
#include <iomanip>
#include <sstream>
#include <fstream>

#include "Beverage.h"
#include "Condiment.h"

// ============================================================================
// ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ
// ============================================================================

// Форматирование цены: 3.2 → "3.20"
std::string formatPrice(double price) {
    std::ostringstream oss;
    oss << std::fixed << std::setprecision(2) << price;
    return oss.str();
}

// ============================================================================
// БАЗЫ ДАННЫХ: напитки и добавки
// ============================================================================

// База данных напитков (в реальном проекте можно вынести в JSON/CSV)
const std::map<std::string, Beverage> BEVERAGES_DB = {
    {"Espresso",    {"Espresso",    "Espresso",    2.50}},
    {"Cappuccino",  {"Cappuccino",  "Cappuccino",  3.50}},
    {"Latte",       {"Latte",       "Latte",       3.20}},
    {"Americano",   {"Americano",   "Americano",   2.80}},
    {"Tea",         {"Tea",         "Tea",         2.00}}
};

// База данных добавок
const std::map<std::string, Condiment> CONDIMENTS_DB = {
    {"Milk",            {"Milk",            0.50}},
    {"Whipped Cream",   {"Whipped Cream",   0.80}},
    {"Vanilla Syrup",   {"Vanilla Syrup",   0.60}},
    {"Caramel Syrup",   {"Caramel Syrup",   0.60}},
    {"Chocolate Syrup", {"Chocolate Syrup", 0.60}},
    {"Sugar",           {"Sugar",           0.10}},
    {"Cinnamon",        {"Cinnamon",        0.30}}
};

// Порядок отображения кнопок (для стабильности)
const std::vector<std::string> BEV_ORDER = { "Espresso", "Cappuccino", "Latte", "Americano", "Tea" };
const std::vector<std::string> COND_ORDER = {
    "Milk", "Whipped Cream", "Vanilla Syrup",
    "Caramel Syrup", "Chocolate Syrup", "Sugar", "Cinnamon"
};

// ============================================================================
// СТРУКТУРА ЗАКАЗА
// ============================================================================

struct OrderItem {
    Beverage beverage;                      // Базовый напиток
    std::map<std::string, int> condimentCounts; // {"Milk": 2, "Sugar": 1}

    OrderItem() {}
    OrderItem(const Beverage& bev) : beverage(bev) {}

    // 🔹 РАСЧЁТ ЦЕНЫ: прямая арифметика (не рекурсия!)
    double getTotalCost() const {
        double total = beverage.basePrice;
        for (const auto& [condName, count] : condimentCounts) {
            auto it = CONDIMENTS_DB.find(condName);
            if (it != CONDIMENTS_DB.end()) {
                total += it->second.price * count;  // цена × количество
            }
        }
        return total;
    }

    // 🔹 СБОРКА ОПИСАНИЯ: цикл по мапе (не цепочка вызовов!)
    std::string getFormattedDescription() const {
        std::string desc = beverage.name;
        for (const auto& [condName, count] : condimentCounts) {
            if (count > 1) {
                desc += ", " + condName + " x" + std::to_string(count);
            }
            else {
                desc += ", " + condName;
            }
        }
        return desc;
    }

    std::string getFullDescription() const {
        return getFormattedDescription() + "\n  Total: $" + formatPrice(getTotalCost());
    }
};

// ============================================================================
// ГЛАВНОЕ ПРИЛОЖЕНИЕ
// ============================================================================

class BeverageApp {
private:
    sf::RenderWindow window;
    sf::Font font;

    // UI элементы
    std::vector<sf::RectangleShape> beverageButtons;
    std::vector<sf::Text> beverageTexts;
    std::vector<sf::RectangleShape> condimentButtons;
    std::vector<sf::Text> condimentTexts;

    // Данные
    std::vector<OrderItem> orders;
    std::string currentBeverageName;  // просто имя, не объект!
    std::map<std::string, int> currentCondimentCounts;

    // История для Undo
    struct HistoryItem {
        std::string type;  // "beverage" или "condiment"
        std::string name;
        double value;
    };
    std::vector<HistoryItem> history;

    // Кнопки действий
    sf::RectangleShape addButton, clearButton, undoButton, checkoutButton, clearAllButton;
    sf::Text addText, clearText, undoText, checkoutText, clearAllText;

    // Тексты
    sf::Text beveragesTitle, condimentsTitle, currentOrderTitle, currentOrderText;
    sf::Text ordersListTitle, ordersListText;

    // Константы вёрстки
    const float BUTTON_W = 200.f, BUTTON_H = 50.f, PAD = 10.f;

public:
    BeverageApp() : window(sf::VideoMode(1000, 800), "Coffee Shop - Simple") {
        window.setFramerateLimit(60);

        // Загрузка шрифта (путь для Windows)
        if (!font.loadFromFile("C:/Windows/Fonts/arial.ttf")) {
            std::cerr << "Error: Could not load font!\n";
            std::cerr << "Make sure arial.ttf exists at C:/Windows/Fonts/\n";
        }

        setupUI();
        updateDisplay();
    }

    // ========================================================================
    // НАСТРОЙКА ИНТЕРФЕЙСА
    // ========================================================================

    void setupUI() {
        // Заголовки секций
        beveragesTitle.setFont(font);
        beveragesTitle.setString("BEVERAGES");
        beveragesTitle.setCharacterSize(20);
        beveragesTitle.setPosition(PAD, 10);
        beveragesTitle.setFillColor(sf::Color::Black);
        beveragesTitle.setStyle(sf::Text::Bold);

        condimentsTitle.setFont(font);
        condimentsTitle.setString("CONDIMENTS");
        condimentsTitle.setCharacterSize(20);
        condimentsTitle.setPosition(PAD, 340);
        condimentsTitle.setFillColor(sf::Color::Black);
        condimentsTitle.setStyle(sf::Text::Bold);

        currentOrderTitle.setFont(font);
        currentOrderTitle.setString("CURRENT ORDER:");
        currentOrderTitle.setCharacterSize(20);
        currentOrderTitle.setPosition(400, 20);
        currentOrderTitle.setFillColor(sf::Color::Black);
        currentOrderTitle.setStyle(sf::Text::Bold);

        ordersListTitle.setFont(font);
        ordersListTitle.setString("ORDERS:");
        ordersListTitle.setCharacterSize(20);
        ordersListTitle.setPosition(400, 400);
        ordersListTitle.setFillColor(sf::Color::Black);
        ordersListTitle.setStyle(sf::Text::Bold);

        // Кнопки напитков
        sf::Color colors[] = {
            sf::Color(101,67,33),   // Espresso - тёмно-коричневый
            sf::Color(193,154,107), // Cappuccino - светло-коричневый
            sf::Color(255,248,220), // Latte - кремовый
            sf::Color(76,60,45),    // Americano - коричневый
            sf::Color(34,139,34)    // Tea - зелёный
        };

        for (size_t i = 0; i < BEV_ORDER.size(); ++i) {
            const auto& bev = BEVERAGES_DB.at(BEV_ORDER[i]);

            sf::RectangleShape btn({ BUTTON_W, BUTTON_H });
            btn.setPosition(PAD, 45 + i * (BUTTON_H + PAD));
            btn.setFillColor(colors[i]);
            btn.setOutlineColor(sf::Color::White);
            btn.setOutlineThickness(2);
            beverageButtons.push_back(btn);

            sf::Text txt(bev.name + " ($" + formatPrice(bev.basePrice) + ")", font, 16);
            txt.setPosition(PAD + 10, 45 + i * (BUTTON_H + PAD) + 12);
            // Чёрный текст на светлом фоне для Latte
            txt.setFillColor(i == 2 ? sf::Color::Black : sf::Color::White);
            beverageTexts.push_back(txt);
        }

        // Кнопки добавок
        for (size_t i = 0; i < COND_ORDER.size(); ++i) {
            const auto& cond = CONDIMENTS_DB.at(COND_ORDER[i]);

            sf::RectangleShape btn({ BUTTON_W, BUTTON_H });
            btn.setPosition(PAD, 375 + i * (BUTTON_H + PAD));
            btn.setFillColor(sf::Color(255, 215, 0));
            btn.setOutlineColor(sf::Color::Black);
            btn.setOutlineThickness(2);
            condimentButtons.push_back(btn);

            sf::Text txt(cond.name + " (+$" + formatPrice(cond.price) + ")", font, 14);
            txt.setPosition(PAD + 10, 375 + i * (BUTTON_H + PAD) + 12);
            txt.setFillColor(sf::Color::Black);
            condimentTexts.push_back(txt);
        }

        // Текстовые поля
        currentOrderText.setFont(font);
        currentOrderText.setCharacterSize(16);
        currentOrderText.setPosition(400, 55);
        currentOrderText.setFillColor(sf::Color::Black);

        ordersListText.setFont(font);
        ordersListText.setCharacterSize(14);
        ordersListText.setPosition(400, 435);

        // Кнопки действий (лямбда для удобства)
        auto makeBtn = [&](sf::RectangleShape& btn, sf::Text& txt, const std::string& label,
            float x, float y, sf::Color clr) {
                btn.setSize({ 180, 45 });
                btn.setPosition(x, y);
                btn.setFillColor(clr);
                txt.setFont(font);
                txt.setString(label);
                txt.setCharacterSize(16);
                txt.setPosition(x + 10, y + 12);
            };

        makeBtn(addButton, addText, "+ Add to Order", 700, 100, sf::Color::Green);
        makeBtn(undoButton, undoText, "< Undo", 700, 160, sf::Color(255, 165, 0));
        makeBtn(clearButton, clearText, "X Clear Current", 700, 220, sf::Color(255, 100, 100));
        makeBtn(clearAllButton, clearAllText, "Clear ALL Orders", 700, 280, sf::Color(128, 128, 128));
        makeBtn(checkoutButton, checkoutText, "$ Checkout", 700, 700, sf::Color::Blue);
    }

    // ========================================================================
    // ОБРАБОТКА СОБЫТИЙ
    // ========================================================================

    void handleEvents() {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                window.close();
            }

            if (event.type != sf::Event::MouseButtonPressed) {
                continue;
            }

            auto pos = sf::Mouse::getPosition(window);

            // Проверка клика по кнопкам напитков
            for (size_t i = 0; i < BEV_ORDER.size(); ++i) {
                if (beverageButtons[i].getGlobalBounds().contains(pos.x, pos.y)) {
                    selectBeverage(BEV_ORDER[i]);
                    return;
                }
            }

            // Проверка клика по кнопкам добавок (только если напиток выбран)
            if (!currentBeverageName.empty()) {
                for (size_t i = 0; i < COND_ORDER.size(); ++i) {
                    if (condimentButtons[i].getGlobalBounds().contains(pos.x, pos.y)) {
                        addCondiment(COND_ORDER[i]);
                        return;
                    }
                }
            }

            // Проверка клика по кнопкам действий
            if (addButton.getGlobalBounds().contains(pos.x, pos.y)) {
                addToOrder();
            }
            if (undoButton.getGlobalBounds().contains(pos.x, pos.y)) {
                undoLast();
            }
            if (clearButton.getGlobalBounds().contains(pos.x, pos.y)) {
                clearCurrent();
            }
            if (clearAllButton.getGlobalBounds().contains(pos.x, pos.y)) {
                clearAllOrders();
            }
            if (checkoutButton.getGlobalBounds().contains(pos.x, pos.y)) {
                checkout();
            }
        }
    }

    // ========================================================================
    // БИЗНЕС-ЛОГИКА
    // ========================================================================

    // Выбор напитка: просто сохраняем имя из базы
    void selectBeverage(const std::string& name) {
        // Сохраняем состояние для undo
        if (!currentBeverageName.empty()) {
            history.push_back({
                "beverage_remove",
                currentBeverageName,
                BEVERAGES_DB.at(currentBeverageName).basePrice
                });
        }

        currentBeverageName = name;
        currentCondimentCounts.clear();
        history.clear();
        history.push_back({ "beverage", name, BEVERAGES_DB.at(name).basePrice });

        updateDisplay();
    }

    // Добавление добавки: просто увеличиваем счётчик в мапе
    void addCondiment(const std::string& name) {
        if (currentBeverageName.empty()) {
            return;
        }
        currentCondimentCounts[name]++;  // просто инкремент!
        history.push_back({ "condiment", name, CONDIMENTS_DB.at(name).price });
        updateDisplay();
    }

    // Отмена последнего действия
    void undoLast() {
        if (history.empty() || currentBeverageName.empty()) {
            return;
        }

        auto last = history.back();
        history.pop_back();

        if (last.type == "beverage") {
            currentBeverageName = "";
            currentCondimentCounts.clear();
        }
        else if (last.type == "beverage_remove") {
            currentBeverageName = last.name;
            currentCondimentCounts.clear();
        }
        else if (last.type == "condiment") {
            if (currentCondimentCounts[last.name] > 0) {
                currentCondimentCounts[last.name]--;
                if (currentCondimentCounts[last.name] == 0) {
                    currentCondimentCounts.erase(last.name);
                }
            }
        }
        updateDisplay();
    }

    // Добавление текущего заказа в список
    void addToOrder() {
        if (currentBeverageName.empty()) {
            return;
        }

        const Beverage& bev = BEVERAGES_DB.at(currentBeverageName);
        OrderItem item(bev);
        item.condimentCounts = currentCondimentCounts;  // копируем мапу

        orders.push_back(item);

        currentBeverageName = "";
        currentCondimentCounts.clear();
        history.clear();

        updateDisplay();
    }

    // Очистка текущего заказа
    void clearCurrent() {
        currentBeverageName = "";
        currentCondimentCounts.clear();
        history.clear();
        updateDisplay();
    }

    // Очистка всех заказов
    void clearAllOrders() {
        orders.clear();
        updateDisplay();
    }

    // Оформление заказа и печать чека
    void checkout() {
        if (orders.empty()) {
            sf::Text msg("No orders!", font, 24);
            msg.setPosition(400, 400);
            msg.setFillColor(sf::Color::Red);

            sf::Clock clk;
            while (clk.getElapsedTime().asSeconds() < 1.5f) {
                sf::Event e;
                while (window.pollEvent(e)) {
                    if (e.type == sf::Event::Closed) {
                        window.close();
                        return;
                    }
                }
                window.clear(sf::Color::White);
                renderUI();
                window.draw(msg);
                window.display();
            }
            return;
        }

        double total = 0;
        std::string receipt = "\n";
        receipt += "===================================\n";
        receipt += "         COFFEE SHOP RECEIPT\n";
        receipt += "===================================\n\n";

        for (size_t i = 0; i < orders.size(); ++i) {
            receipt += "+ Order #" + std::to_string(i + 1) + "\n";
            receipt += "| " + orders[i].getFullDescription() + "\n";
            receipt += "+---------------------------------\n\n";
            total += orders[i].getTotalCost();
        }

        receipt += "===================================\n";
        receipt += "  ITEMS: " + std::to_string(orders.size()) + "\n";
        receipt += "  GRAND TOTAL: $" + formatPrice(total) + "\n";
        receipt += "===================================\n\n";
        receipt += "  Thank you for your order!\n";
        receipt += "  Come back soon!\n";

        // 1. Вывод в консоль
        std::cout << receipt << std::endl;

        // 2. Запись в файл (имитация печати)
        std::ofstream receiptFile("receipt.txt");
        if (receiptFile.is_open()) {
            receiptFile << receipt;
            receiptFile.close();
            std::cout << "[Receipt saved to receipt.txt]" << std::endl;
        }
        else {
            std::cerr << "Error: Could not create receipt.txt" << std::endl;
        }

        orders.clear();

        // 3. Показать сообщение в окне
        sf::Text msg("Checkout complete!\nTotal: $" + formatPrice(total) +
            "\n\nReceipt saved to receipt.txt", font, 18);
        msg.setPosition(350, 350);
        msg.setFillColor(sf::Color::Black);

        sf::Clock clk;
        while (clk.getElapsedTime().asSeconds() < 3.f) {
            sf::Event e;
            while (window.pollEvent(e)) {
                if (e.type == sf::Event::Closed) {
                    window.close();
                    return;
                }
            }
            window.clear(sf::Color(240, 240, 240));
            window.draw(msg);
            window.display();
        }
        updateDisplay();
    }

    // Обновление отображения
    void updateDisplay() {
        // Текущий заказ
        if (currentBeverageName.empty()) {
            currentOrderText.setString("Select a beverage");
            currentOrderText.setFillColor(sf::Color(128, 128, 128));
        }
        else {
            // Собираем описание вручную
            std::string desc = currentBeverageName;
            for (const auto& [condName, count] : currentCondimentCounts) {
                if (count > 1) {
                    desc += ", " + condName + " x" + std::to_string(count);
                }
                else {
                    desc += ", " + condName;
                }
            }

            // Считаем цену арифметически
            double price = BEVERAGES_DB.at(currentBeverageName).basePrice;
            for (const auto& [condName, count] : currentCondimentCounts) {
                price += CONDIMENTS_DB.at(condName).price * count;
            }

            desc += "\n\n$" + formatPrice(price);
            currentOrderText.setString(desc);
            currentOrderText.setFillColor(sf::Color::Black);
        }

        // Список заказов
        std::string list;
        double total = 0;

        if (orders.empty()) {
            list = "(empty)";
            ordersListText.setFillColor(sf::Color(128, 128, 128));
        }
        else {
            ordersListText.setFillColor(sf::Color::Black);
            for (size_t i = 0; i < orders.size(); ++i) {
                list += "#" + std::to_string(i + 1) + ": " + orders[i].getFormattedDescription() +
                    " [$" + formatPrice(orders[i].getTotalCost()) + "]\n";
                total += orders[i].getTotalCost();
            }
            list += "\nTOTAL: $" + formatPrice(total);
        }
        ordersListText.setString(list);
    }

    // Отрисовка интерфейса
    void renderUI() {
        window.draw(beveragesTitle);
        window.draw(condimentsTitle);
        window.draw(currentOrderTitle);
        window.draw(ordersListTitle);

        for (size_t i = 0; i < beverageButtons.size(); ++i) {
            window.draw(beverageButtons[i]);
            window.draw(beverageTexts[i]);
        }
        for (size_t i = 0; i < condimentButtons.size(); ++i) {
            window.draw(condimentButtons[i]);
            window.draw(condimentTexts[i]);
        }

        window.draw(currentOrderText);
        window.draw(ordersListText);
        window.draw(addButton);
        window.draw(addText);
        window.draw(undoButton);
        window.draw(undoText);
        window.draw(clearButton);
        window.draw(clearText);
        window.draw(clearAllButton);
        window.draw(clearAllText);
        window.draw(checkoutButton);
        window.draw(checkoutText);
    }

    // Главный цикл приложения
    void run() {
        while (window.isOpen()) {
            handleEvents();
            window.clear(sf::Color::White);
            renderUI();
            window.display();
        }
    }

    ~BeverageApp() = default;
};

// ============================================================================
// ТОЧКА ВХОДА
// ============================================================================

int main() {
    try {
        BeverageApp app;
        app.run();
    }
    catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1;
    }
    return 0;
}