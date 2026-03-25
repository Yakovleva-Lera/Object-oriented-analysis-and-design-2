#ifndef CONDIMENT_DECORATOR_H
#define CONDIMENT_DECORATOR_H

#include "Beverage.h"
#include <memory>
#include <string>

class CondimentDecorator : public Beverage {
protected:
    std::unique_ptr<Beverage> beverage;
    std::string condimentName;
    double condimentCost;

public:
    CondimentDecorator(std::unique_ptr<Beverage> bev,
        const std::string& name, double cost)
        : beverage(std::move(bev)), condimentName(name), condimentCost(cost) {
    }

    // Деструктор не нужен: unique_ptr автоматически удалит beverage

    std::string getDescription() const override {
        return beverage->getDescription() + ", " + condimentName;
    }

    double getCost() const override {
        return beverage->getCost() + condimentCost;
    }

    std::string getName() const override {
        return beverage->getName();
    }

    std::string getCondimentName() const { return condimentName; }
    double getCondimentCost() const { return condimentCost; }
};

#endif // CONDIMENT_DECORATOR_H