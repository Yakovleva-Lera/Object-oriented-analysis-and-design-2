#ifndef CONDIMENT_H
#define CONDIMENT_H

#include <string>

// Простая структура: имя добавки и её цена
struct Condiment {
    std::string name;   // "Milk", "Sugar"
    double price;       // 0.50, 0.10

    Condiment() : name(""), price(0.0) {}

    Condiment(const std::string& n, double p) : name(n), price(p) {}
};

#endif // CONDIMENT_H