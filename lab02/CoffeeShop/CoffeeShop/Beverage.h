#ifndef BEVERAGE_H
#define BEVERAGE_H

#include <string>

// Простая структура с данными о напитке
// Без виртуальных методов, без наследования
struct Beverage {
    std::string name;        // "Latte", "Espresso"
    std::string description; // "Latte"
    double basePrice;        // 3.20

    Beverage() : name("Unknown"), description("Unknown beverage"), basePrice(0.0) {}

    Beverage(const std::string& n, const std::string& desc, double price)
        : name(n), description(desc), basePrice(price) {
    }
};

#endif // BEVERAGE_H