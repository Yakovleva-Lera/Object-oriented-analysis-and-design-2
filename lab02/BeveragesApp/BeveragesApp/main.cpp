#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <map>
#include <memory>
#include <functional>
#include <iomanip>
#include <sstream>
#include <fstream>

#include "Beverage.h"
#include "ConcreteBeverages.h"
#include "ConcreteCondiments.h"

// Форматирование цены
std::string formatPrice(double price) {
    std::ostringstream oss;
    oss << std::fixed << std::setprecision(2) << price;
    return oss.str();
}

struct OrderItem {
    std::unique_ptr<Beverage> beverage;
    std::string baseDescription;
    std::map<std::string, int> condimentCounts;

    OrderItem(std::unique_ptr<Beverage> bev, const std::string& desc,
        const std::map<std::string, int>& condiments)
        : beverage(std::move(bev)), baseDescription(desc), condimentCounts(condiments) {
    }

    double getTotalCost() const {
        return beverage ? beverage->getCost() : 0.0;
    }

    std::string getFormattedDescription() const {
        std::string desc = baseDescription;
        for (const auto& pair : condimentCounts) {
            if (pair.second > 1) {
                desc += ", " + pair.first + " x" + std::to_string(pair.second);
            }
            else {
                desc += ", " + pair.first;
            }
        }
        return desc;
    }

    std::string getFullDescription() const {
        return getFormattedDescription() + "\n  Total: $" + formatPrice(getTotalCost());
    }
};

class BeverageApp {
private:
    sf::RenderWindow window;
    sf::Font font;

    std::vector<sf::RectangleShape> beverageButtons;
    std::vector<sf::Text> beverageTexts;
    std::vector<sf::RectangleShape> condimentButtons;
    std::vector<sf::Text> condimentTexts;

    std::vector<OrderItem> orders;
    std::unique_ptr<Beverage> currentBeverage;
    std::string currentBeverageBaseName;
    std::map<std::string, int> currentCondimentCounts;

    struct HistoryItem {
        std::string type;
        std::string name;
        double cost;
    };
    std::vector<HistoryItem> history;

    // Кнопки
    sf::RectangleShape addButton, clearButton, undoButton, checkoutButton, clearAllButton;
    sf::Text addText, clearText, undoText, checkoutText, clearAllText;

    // Тексты
    sf::Text beveragesTitle, condimentsTitle, currentOrderTitle, currentOrderText;
    sf::Text ordersListTitle, ordersListText;

    const float BUTTON_W = 200.f, BUTTON_H = 50.f, PAD = 10.f;

    // Фабрики напитков
    const std::vector<std::function<std::unique_ptr<Beverage>()>> bevFactories = {
        []() { return std::make_unique<Espresso>(); },
        []() { return std::make_unique<Cappuccino>(); },
        []() { return std::make_unique<Latte>(); },
        []() { return std::make_unique<Americano>(); },
        []() { return std::make_unique<Tea>(); }
    };
    const std::vector<std::string> bevNames = { "Espresso", "Cappuccino", "Latte", "Americano", "Tea" };
    const std::vector<double> bevCosts = { 2.50, 3.50, 3.20, 2.80, 2.00 };

    // Фабрики добавок
    const std::vector<std::function<std::unique_ptr<Beverage>(std::unique_ptr<Beverage>)>> condFactories = {
        [](auto b) { return std::make_unique<Milk>(std::move(b)); },
        [](auto b) { return std::make_unique<WhippedCream>(std::move(b)); },
        [](auto b) { return std::make_unique<VanillaSyrup>(std::move(b)); },
        [](auto b) { return std::make_unique<CaramelSyrup>(std::move(b)); },
        [](auto b) { return std::make_unique<ChocolateSyrup>(std::move(b)); },
        [](auto b) { return std::make_unique<Sugar>(std::move(b)); },
        [](auto b) { return std::make_unique<Cinnamon>(std::move(b)); }
    };
    const std::vector<std::string> condNames = {
        "Milk", "Whipped Cream", "Vanilla Syrup",
        "Caramel Syrup", "Chocolate Syrup", "Sugar", "Cinnamon"
    };
    const std::vector<double> condCosts = { 0.50, 0.80, 0.60, 0.60, 0.60, 0.10, 0.30 };

public:
    BeverageApp() : window(sf::VideoMode(1000, 800), "Coffee Shop - Decorator") {
        window.setFramerateLimit(60);
        if (!font.loadFromFile("C:/Windows/Fonts/arial.ttf"))
            std::cerr << "Font error!\n";
        setupUI();
        updateDisplay();
    }

    void setupUI() {
        // Заголовки
        beveragesTitle.setFont(font); beveragesTitle.setString("BEVERAGES");
        beveragesTitle.setCharacterSize(20); beveragesTitle.setPosition(PAD, 10);
        beveragesTitle.setFillColor(sf::Color::Black); beveragesTitle.setStyle(sf::Text::Bold);

        condimentsTitle.setFont(font); condimentsTitle.setString("CONDIMENTS");
        condimentsTitle.setCharacterSize(20); condimentsTitle.setPosition(PAD, 340);
        condimentsTitle.setFillColor(sf::Color::Black); condimentsTitle.setStyle(sf::Text::Bold);

        currentOrderTitle.setFont(font); currentOrderTitle.setString("CURRENT ORDER:");
        currentOrderTitle.setCharacterSize(20); currentOrderTitle.setPosition(400, 20);
        currentOrderTitle.setFillColor(sf::Color::Black); currentOrderTitle.setStyle(sf::Text::Bold);

        ordersListTitle.setFont(font); ordersListTitle.setString("ORDERS:");
        ordersListTitle.setCharacterSize(20); ordersListTitle.setPosition(400, 400);
        ordersListTitle.setFillColor(sf::Color::Black); ordersListTitle.setStyle(sf::Text::Bold);

        // Кнопки напитков
        sf::Color colors[] = {
            sf::Color(101,67,33), sf::Color(193,154,107), sf::Color(255,248,220),
            sf::Color(76,60,45), sf::Color(34,139,34)
        };
        for (int i = 0; i < 5; ++i) {
            sf::RectangleShape btn({ BUTTON_W, BUTTON_H });
            btn.setPosition(PAD, 45 + i * (BUTTON_H + PAD));
            btn.setFillColor(colors[i]); btn.setOutlineColor(sf::Color::White); btn.setOutlineThickness(2);
            beverageButtons.push_back(btn);

            sf::Text txt(bevNames[i], font, 18);
            txt.setPosition(PAD + 10, 45 + i * (BUTTON_H + PAD) + 12);
            txt.setFillColor(i == 2 ? sf::Color::Black : sf::Color::White);
            beverageTexts.push_back(txt);
        }

        // Кнопки добавок
        for (int i = 0; i < 7; ++i) {
            sf::RectangleShape btn({ BUTTON_W, BUTTON_H });
            btn.setPosition(PAD, 375 + i * (BUTTON_H + PAD));
            btn.setFillColor(sf::Color(255, 215, 0)); btn.setOutlineColor(sf::Color::Black); btn.setOutlineThickness(2);
            condimentButtons.push_back(btn);

            sf::Text txt(condNames[i] + " (+$" + formatPrice(condCosts[i]) + ")", font, 14);
            txt.setPosition(PAD + 10, 375 + i * (BUTTON_H + PAD) + 12);
            txt.setFillColor(sf::Color::Black);
            condimentTexts.push_back(txt);
        }

        // Текущий заказ
        currentOrderText.setFont(font); currentOrderText.setCharacterSize(16);
        currentOrderText.setPosition(400, 55); currentOrderText.setFillColor(sf::Color::Black);

        // Список заказов
        ordersListText.setFont(font); ordersListText.setCharacterSize(14);
        ordersListText.setPosition(400, 435);

        // Кнопки действий
        auto makeBtn = [&](sf::RectangleShape& btn, sf::Text& txt, const std::string& label, float x, float y, sf::Color clr) {
            btn.setSize({ 180, 45 }); btn.setPosition(x, y); btn.setFillColor(clr);
            txt.setFont(font); txt.setString(label); txt.setCharacterSize(16);
            txt.setPosition(x + 10, y + 12);
            };
        makeBtn(addButton, addText, "+ Add to Order", 700, 100, sf::Color::Green);
        makeBtn(undoButton, undoText, "< Undo", 700, 160, sf::Color(255, 165, 0));
        makeBtn(clearButton, clearText, "X Clear Current", 700, 220, sf::Color(255, 100, 100));
        makeBtn(clearAllButton, clearAllText, "Clear ALL Orders", 700, 280, sf::Color(128, 128, 128));
        makeBtn(checkoutButton, checkoutText, "$ Checkout", 700, 700, sf::Color::Blue);
    }

    void handleEvents() {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) window.close();
            if (event.type != sf::Event::MouseButtonPressed) continue;

            auto pos = sf::Mouse::getPosition(window);

            // Напитки
            for (int i = 0; i < 5; ++i)
                if (beverageButtons[i].getGlobalBounds().contains(pos.x, pos.y)) { selectBeverage(i); return; }

            // Добавки
            if (currentBeverage)
                for (int i = 0; i < 7; ++i)
                    if (condimentButtons[i].getGlobalBounds().contains(pos.x, pos.y)) { addCondiment(i); return; }

            // Кнопки
            if (addButton.getGlobalBounds().contains(pos.x, pos.y)) addToOrder();
            if (undoButton.getGlobalBounds().contains(pos.x, pos.y)) undoLast();
            if (clearButton.getGlobalBounds().contains(pos.x, pos.y)) clearCurrent();
            if (clearAllButton.getGlobalBounds().contains(pos.x, pos.y)) clearAllOrders();
            if (checkoutButton.getGlobalBounds().contains(pos.x, pos.y)) checkout();
        }
    }

    void selectBeverage(int idx) {
        if (currentBeverage) history.push_back({ "beverage_remove", currentBeverageBaseName, currentBeverage->getCost() });
        currentBeverage = bevFactories[idx]();
        currentBeverageBaseName = bevNames[idx];
        currentCondimentCounts.clear();
        history.clear();
        history.push_back({ "beverage", currentBeverageBaseName, bevCosts[idx] });
        updateDisplay();
    }

    void addCondiment(int idx) {
        if (!currentBeverage) return;
        currentBeverage = condFactories[idx](std::move(currentBeverage));
        currentCondimentCounts[condNames[idx]]++;
        history.push_back({ "condiment", condNames[idx], condCosts[idx] });
        updateDisplay();
    }

    void undoLast() {
        if (history.empty() || !currentBeverage) return;
        auto last = history.back(); history.pop_back();

        if (last.type == "beverage") {
            currentBeverage.reset();
            currentBeverageBaseName = "";
            currentCondimentCounts.clear();
        }
        else if (last.type == "beverage_remove") {
            for (size_t i = 0; i < bevNames.size(); ++i)
                if (bevNames[i] == last.name) {
                    currentBeverage = bevFactories[i]();
                    currentBeverageBaseName = last.name;
                    currentCondimentCounts.clear();
                    break;
                }
        }
        else if (last.type == "condiment") {
            if (currentCondimentCounts[last.name] > 0) {
                currentCondimentCounts[last.name]--;
                if (currentCondimentCounts[last.name] == 0) {
                    currentCondimentCounts.erase(last.name);
                }
            }

            for (size_t i = 0; i < bevNames.size(); ++i) {
                if (bevNames[i] == currentBeverageBaseName) {
                    currentBeverage = bevFactories[i]();
                    for (const auto& h : history) {
                        if (h.type == "condiment") {
                            for (int k = 0; k < 7; ++k) {
                                if (condNames[k] == h.name) {
                                    currentBeverage = condFactories[k](std::move(currentBeverage));
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        updateDisplay();
    }

    void addToOrder() {
        if (!currentBeverage) return;
        orders.emplace_back(std::move(currentBeverage), currentBeverageBaseName,
            currentCondimentCounts);
        currentBeverage.reset();
        currentBeverageBaseName = "";
        history.clear();
        currentCondimentCounts.clear();
        updateDisplay();
    }

    void clearCurrent() {
        currentBeverage.reset();
        currentBeverageBaseName = "";
        history.clear();
        currentCondimentCounts.clear();
        updateDisplay();
    }

    void clearAllOrders() {
        orders.clear();
        updateDisplay();
    }

    void checkout() {
        if (orders.empty()) {
            sf::Text msg("No orders!", font, 24); msg.setPosition(400, 400); msg.setFillColor(sf::Color::Red);
            sf::Clock clk;
            while (clk.getElapsedTime().asSeconds() < 1.5f) {
                sf::Event e; while (window.pollEvent(e)) if (e.type == sf::Event::Closed) window.close();
                window.clear(sf::Color::White); renderUI(); window.draw(msg); window.display();
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

        // Для окна используем текст без сложных рамок, чтобы не было проблем
        std::string windowMsg = "Checkout complete!\nTotal: $" + formatPrice(total) + "\n\nReceipt saved to receipt.txt";
        sf::Text msg(windowMsg, font, 18);
        msg.setPosition(350, 350);
        msg.setFillColor(sf::Color::Black);

        sf::Clock clk;
        while (clk.getElapsedTime().asSeconds() < 3.f) {
            sf::Event e; while (window.pollEvent(e)) if (e.type == sf::Event::Closed) window.close();
            window.clear(sf::Color(240, 240, 240));
            window.draw(msg);
            window.display();
        }
        updateDisplay();
    }

    void updateDisplay() {
        // Текущий заказ
        if (!currentBeverage) {
            currentOrderText.setString("Select a beverage");
            currentOrderText.setFillColor(sf::Color(128, 128, 128));
        }
        else {
            std::string desc = currentBeverageBaseName;
            for (const auto& pair : currentCondimentCounts) {
                if (pair.second > 1) {
                    desc += ", " + pair.first + " x" + std::to_string(pair.second);
                }
                else {
                    desc += ", " + pair.first;
                }
            }
            desc += "\n\n$" + formatPrice(currentBeverage->getCost());
            currentOrderText.setString(desc);
            currentOrderText.setFillColor(sf::Color::Black);
        }

        // Список заказов (с группировкой добавок)
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

int main() {
    BeverageApp app;
    app.run();
    return 0;
}