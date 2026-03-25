#ifndef CONCRETE_CONDIMENTS_H
#define CONCRETE_CONDIMENTS_H

#include "CondimentDecorator.h"
#include <memory>

class Milk : public CondimentDecorator {
public:
    Milk(std::unique_ptr<Beverage> bev)
        : CondimentDecorator(std::move(bev), "Milk", 0.50) {
    }
};

class WhippedCream : public CondimentDecorator {
public:
    WhippedCream(std::unique_ptr<Beverage> bev)
        : CondimentDecorator(std::move(bev), "Whipped Cream", 0.80) {
    }
};

class VanillaSyrup : public CondimentDecorator {
public:
    VanillaSyrup(std::unique_ptr<Beverage> bev)
        : CondimentDecorator(std::move(bev), "Vanilla Syrup", 0.60) {
    }
};

class CaramelSyrup : public CondimentDecorator {
public:
    CaramelSyrup(std::unique_ptr<Beverage> bev)
        : CondimentDecorator(std::move(bev), "Caramel Syrup", 0.60) {
    }
};

class ChocolateSyrup : public CondimentDecorator {
public:
    ChocolateSyrup(std::unique_ptr<Beverage> bev)
        : CondimentDecorator(std::move(bev), "Chocolate Syrup", 0.60) {
    }
};

class Sugar : public CondimentDecorator {
public:
    Sugar(std::unique_ptr<Beverage> bev)
        : CondimentDecorator(std::move(bev), "Sugar", 0.10) {
    }
};

class Cinnamon : public CondimentDecorator {
public:
    Cinnamon(std::unique_ptr<Beverage> bev)
        : CondimentDecorator(std::move(bev), "Cinnamon", 0.30) {
    }
};

#endif // CONCRETE_CONDIMENTS_H