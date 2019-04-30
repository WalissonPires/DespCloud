function strDecimalTostrDecimalBr(strFloat) {
    return strFloat.replace('.', '').replace(',', '.');
}

/*
 * Localized default methods for the jQuery validation plugin.
 * Locale: PT_BR
 */
$.extend( $.validator.methods, {
	//date: function (value, element) {
    //    return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    //},
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    },
    min: function (value, element, param) {
        return this.optional(element) || strDecimalTostrDecimalBr(value) >= param;
    },
    max: function (value, element, param) {
        return this.optional(element) || strDecimalTostrDecimalBr(value) <= param;
    },
    selectRequired: function(value, element) {  
      return (typeof value === 'string' && value.length > 0 && value !== '#new');
    }
} );


//jQuery.validator.addMethod("selectRequired", function(value, element) {
//  return (typeof value === 'string' && value.length > 0 && value !== '#new');
//}, 'Esse campo é requirido');