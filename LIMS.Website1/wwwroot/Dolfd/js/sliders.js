var plugin_path = 'Dolfd/js/';
var slideIndex = 0;
showSlides();

function showSlides() {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("dot");
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slideIndex++;
    if (slideIndex > slides.length) { slideIndex = 1 }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" activedot", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " activedot";
    setTimeout(showSlides, 4000); // Change image every 2 seconds
}

var slideIndex1 = 1;
showDivs(slideIndex1);

function plusDivs(n) {
    showDivs(slideIndex1 += n);
}

function showDivs(n) {
    var i;
    var x = document.getElementsByClassName("mySlides");
    if (n > x.length) { slideIndex1 = 1 }
    if (n < 1) { slideIndex1 = x.length }
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }
    x[slideIndex1 - 1].style.display = "block";
}