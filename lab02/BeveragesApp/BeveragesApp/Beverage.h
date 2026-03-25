#ifndef BEVERAGE_H
#define BEVERAGE_H

#include <string>

class Beverage {
protected:
    std::string description;

public:
    Beverage() : description("Unknown beverage") {}
    virtual ~Beverage() = default;

    virtual std::string getDescription() const { return description; }
    virtual double getCost() const = 0;
    virtual std::string getName() const = 0;
};

#endif // BEVERAGE_H