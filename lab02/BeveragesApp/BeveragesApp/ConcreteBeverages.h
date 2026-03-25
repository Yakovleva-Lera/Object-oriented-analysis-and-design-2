#ifndef CONCRETE_BEVERAGES_H
#define CONCRETE_BEVERAGES_H

#include "Beverage.h"

class Espresso : public Beverage {
public:
    Espresso() { description = "Espresso"; }
    double getCost() const override { return 2.50; }
    std::string getName() const override { return "Espresso"; }
};

class Cappuccino : public Beverage {
public:
    Cappuccino() { description = "Cappuccino"; }
    double getCost() const override { return 3.50; }
    std::string getName() const override { return "Cappuccino"; }
};

class Latte : public Beverage {
public:
    Latte() { description = "Latte"; }
    double getCost() const override { return 3.20; }
    std::string getName() const override { return "Latte"; }
};

class Americano : public Beverage {
public:
    Americano() { description = "Americano"; }
    double getCost() const override { return 2.80; }
    std::string getName() const override { return "Americano"; }
};

class Tea : public Beverage {
public:
    Tea() { description = "Tea"; }
    double getCost() const override { return 2.00; }
    std::string getName() const override { return "Tea"; }
};

#endif // CONCRETE_BEVERAGES_H