var oneWeekTable, twoWeekTable;
function OneWeek() {
    oneWeekTable = document.getElementsByClassName("oneWeekTable");
    console.log(oneWeekTable);

    twoWeekTable = document.getElementsByClassName("twoWeekTable");
    console.log(twoWeekTable);

if (oneWeekTable[0].hasAttribute("hidden") == true) {
    for (var i = 0; i < document.getElementsByClassName("twoWeekTable").length; i++) {
        twoWeekTable[i].setAttribute("hidden", "hidden");
    }
    for (var i = 0; i < document.getElementsByClassName("oneWeekTable").length; i++) {
        oneWeekTable[i].removeAttribute("hidden");
    }
    }
}
function TwoWeek() {
    oneWeekTable = document.getElementsByClassName("oneWeekTable");
    console.log(oneWeekTable);

    twoWeekTable = document.getElementsByClassName("twoWeekTable");
    console.log(twoWeekTable);

if (twoWeekTable[0].hasAttribute("hidden") == true) {
    for (var i = 0; i < document.getElementsByClassName("oneWeekTable").length; i++) {
        oneWeekTable[i].setAttribute("hidden", "hidden");
    }
    for (var i = 0; i < document.getElementsByClassName("twoWeekTable").length; i++) {
        twoWeekTable[i].removeAttribute("hidden");
    }
}
}
$('#buttonModal').click(function () {
    if ($("#menuToggle>input").is(':checked') == true) {
        $(".navbar").slideUp();
    }
    $('.modalSchedule').addClass('active');
    $('.modal1').addClass('active');
});

$('#buttonModal').click(function () {
    $('.modalSchedule').addClass('active');
    $('.modal1').addClass('active');;
});

$('.close-modal1').click(function () {
    $('.modalSchedule').removeClass('active');
    $('.modal1').removeClass('active');
});

$('#buttonModal2').click(function () {
    $('.modalFolderAdd').addClass('active');
    $('.modal2').addClass('active');
});

$('.close-modal2').click(function () {
    $('.modalFolderAdd').removeClass('active');
    $('.modal2').removeClass('active');
});

$('#buttonModal3').click(function () {
    $('.modalFileAdd').addClass('active');
    $('.modal3').addClass('active');
});

$('.close-modal3').click(function () {
    $('.modalFileAdd').removeClass('active');
    $('.modal3').removeClass('active');
});

$('#buttonModal4').click(function () {
    $('.modalAdvertisementAdd').addClass('active');
    $('.modal4').addClass('active');
});

$('.close-modal4').click(function () {
    $('.modalAdvertisementAdd').removeClass('active');
    $('.modal4').removeClass('active');
});

$('#btnInfoСlass').click(function () {
    $('.modalUpdate').addClass('active');
    $('.modal5').addClass('active');
});
$('#btnInfoСlassFile').click(function () {
    $('.modalUpdate').addClass('active');
    $('.modal5').addClass('active');
});
$('.close-modal5').click(function () {
    $('.modalUpdate').removeClass('active');
    $('.modal5').removeClass('active');
});

$('#btnInfoAdvert').click(function () {
    $('.modalUpdateAdvert').addClass('active');
    $('.modal6').addClass('active');
});
$('.close-modal6').click(function () {
    $('.modalUpdateAdvert').removeClass('active');
    $('.modal6').removeClass('active');
});

$('#buttonModal7').click(function () {
    $('.modalGraphicAdd').addClass('active');
    $('.modal7').addClass('active');
});
$('.close-modal7').click(function () {
    $('.modalGraphicAdd').removeClass('active');
    $('.modal7').removeClass('active');
});

$('#btnInfoGraphic').click(function () {
    $('.modalUpdateGraphic').addClass('active');
    $('.modal8').addClass('active');
});
$('.close-modal8').click(function () {
    $('.modalUpdateGraphic').removeClass('active');
    $('.modal8').removeClass('active');
});

function clickAnswer(e) {
    var parentId = e.target.parentNode.id;
    if (e.target.getAttribute('data-trueanswer') == 'True') {
    e.target.setAttribute("class", "trueAnswer");
    }
    else {
    e.target.setAttribute("class", "answer");
    }

    for (var i = 0; i < 5; i++) {
        if ($("#" + parentId + "> .hoverAnswer").eq(i).attr('data-trueAnswer') == 'True') {
            $("#" + parentId + "> .hoverAnswer").eq(i).attr('class', 'trueAnswer');
        }
    }
        $("#" + parentId + "> .hoverAnswer").unbind();
}
$(".hoverAnswer").one("click", clickAnswer);