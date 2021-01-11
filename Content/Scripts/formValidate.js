// Переопределение валидации дробных чисел для массива
function validateRange(value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
} // validateRange

// Переопределение валидации дробных чисел для числа
function validateNumber(value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
} // validateNumber