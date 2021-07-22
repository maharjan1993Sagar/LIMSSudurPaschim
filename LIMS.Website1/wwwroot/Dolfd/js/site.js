$('#myTab li a').click(function (e) {
    $('#myTab li a.active').removeClass('active');
   
    $(this).addClass('active');
    
});

$('#myTabNews li a').click(function (e) {
    $('#myTabNews li a.active').removeClass('active');
  
    $(this).addClass('active');
   
});

$(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    var formatToday = yyyy + '-' + mm + '-' + dd;
    var todayBS = EngToNep(AD2BS(formatToday));

    var index = today.getDay();
    var weekDay = getDay(index);
        
    $("body .dateandtime span").html(weekDay+','+todayBS);

})


$(".lowBandwidth").click(function (e) {

    location.reload();

    // Get class list string
    //var classList =$(this).attr("class");

    //// Creating class array by splitting class list string
    //var classArr = classList.split(/\s+/);

    //if (!classArr.includes('noImg')) {

    //    $(this).addClass('noImg');
    //    var images = document.getElementsByTagName('img');
    //    var l = images.length;
    //    for (var i = 0; i < l; i++) {
    //        images[0].parentNode.removeChild(images[0]);
    //    }

    //}
    //else {

    //    location.reload();
    //}
})


$("document").ready(function () {
    $("ul.nav-second-main").remove();
})
