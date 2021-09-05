var fontSize = 100;

$(document).ready(function () {

    if (readCookie("fontSize") != null) {
        var fontSize = readCookie("fontSize");
    } else {
        var fontSize = 100;
    }
    createCookie("fontSize", fontSize);

    $('.container, .mb-title, .navbar-nav > li > a, p, .innercontent, .links ul li a, .links, .copyright, .homecontact span, .event-txt, .eventtxt a, .press-txt a, .eventtxt a').css("font-size", fontSize + "%");

    $('#btnDecFont').on('click', function (e) {
        set_font_size('decrease');
    });
    $('#btnNorFont').on('click', function (e) {
        set_font_size('normal');
    });
    $('#btnIncFont').on('click', function (e) {
        set_font_size('increase');
    });
});
function set_font_size(fontType) {
    if (fontType == "increase") {
        if (fontSize < 111) {
            fontSize = parseInt(fontSize) + 4;
        }
    } else if (fontType == "decrease") {
        if (fontSize > 89) {
            fontSize = parseInt(fontSize) - 4;
        }
    } else {
        fontSize = 100;
    }
    createCookie("fontSize", fontSize);
    $('body').css('font-size', fontSize + "%");
   // $('.container, .mb-title, .navbar-nav > li > a, p, .innercontent, .links ul li a, .links, .copyright, .homecontact span, .event-txt, .eventtxt a, .press-txt a, .eventtxt a').css("font-size", fontSize + "%");
}

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}


function setActiveStyleSheet(title) {
    var i, a, main;
    for (i = 0; (a = document.getElementsByTagName("link")[i]); i++) {
        if (a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title")) {
            a.disabled = true;
            if (a.getAttribute("title") == title) a.disabled = false;
        }
    }
}
