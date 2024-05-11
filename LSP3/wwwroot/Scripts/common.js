/*  Common Functions */

/*
Functions:

    GetParameterByName - retrieves value of a querystring variable
    toProperCase - input string, output proper cased string
    htmlEscape - html encode special characters
    getCurrentDate - return today's date in yyyy/mm/dd format
    isBlankOrNull - returns true if blank or null 
    checkBoolean - 1 if checkmark checked, 0 otherwise
    checkText - Return default value if blank or null
    getCurrentDate - return today's date in yyyy/mm/dd format   
*/

var API_URL = 'http://164.92.99.186:8080/api';

/*     GetParameterByName - retrieves value of a querystring variable */
function GetParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");

    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);

    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


/* toProperCase - input string, output proper cased string */
function toProperCase(inString) {
    var i, str, lowers, uppers;

    str = inString.replace(/([^\W_]+[^\s-]*) */g, function (txt) {
        return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
    });

    // Certain minor words should be left lowercase unless 
    // they are the first or last words in the string
    lowers = [
        'A', 'An', 'The', 'And', 'But', 'Or', 'For', 'Nor', 'As', 'At',
        'By', 'For', 'From', 'In', 'Into', 'Near', 'Of', 'On', 'Onto', 'To', 'With'
    ];

    for (i = 0; i < lowers.length; i++)
        str = str.replace(new RegExp('\\s' + lowers[i] + '\\s', 'g'),
            function (txt) {
                return txt.toLowerCase();
            });

    // Certain words such as initialisms or acronyms should be left uppercase
    uppers = ['Id', 'Tv'];

    for (i = 0; i < uppers.length; i++)
        str = str.replace(new RegExp('\\b' + uppers[i] + '\\b', 'g'),
            uppers[i].toUpperCase());

    return str;
}

/* htmlEscape - html encode special characters */
function htmlEscape(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

/* isBlankOrNull - returns true if blank or null */
function isBlankOrNull(tag) {
    if ($.trim($(tag).val()) === '') {
        return true;
    } else {
        return false;
    }
}

/* checkBoolean - 1 if checkmark checked, 0 otherwise */
function checkBoolean(tag) {
    return $(tag).prop('checked') ? 1 : 0;
}

/* checkText - Return default value if blank or null */
function checkText(tag, defvalue) {
    return isBlankOrNull(tag) ? defvalue : $(tag).val();
}


/* getCurrentDate - return today's date in yyyy/mm/dd format */
function getCurrentDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!`

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today1 = yyyy + '/' + mm + '/' + dd;
    return today1;
}

