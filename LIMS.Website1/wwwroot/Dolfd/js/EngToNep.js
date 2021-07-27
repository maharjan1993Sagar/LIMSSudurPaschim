function EngToNep(str) {
    //var nepaliMonths = getNepaliMonthsInNepali();

    var returnStr = "";
    for (var i = 0; i < str.length; i++) {
        if (str[i] == 0) {
            returnStr += "०";
        }
        else if (str[i] == 1) {
            returnStr += "१";

        } else if (str[i] == 2) {
            returnStr += "२";

        } else if (str[i] == 3) {
            returnStr += "३";

        } else if (str[i] == 4) {
            returnStr += "४";

        } else if (str[i] == 5) {
            returnStr += "५";

        } else if (str[i] == 6) {
            returnStr += "६";

        } else if (str[i] == 7) {
            returnStr += "७";

        } else if (str[i] == 8) {
            returnStr += "८";

        } else if (str[i] == 9) {
            returnStr += "९";

        }
        else {
            returnStr += str[i];

        }
    }
    return returnStr;
}

function getDay(index) {
    //var daysEng = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'];
    //daysEng.indexOf(day.toLowerCase());
    var intIndex = parseInt(index);
    var days = ['आइतबार', 'सोमबार', 'मंगलबार', 'बुधबार',' बिहिबार', 'शुक्रबार', 'शनिबार'];
    return days[intIndex];
}
