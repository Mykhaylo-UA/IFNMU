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

var open = true;
var ulnav = document.getElementsByClassName("ulnav")[0];
var navigation = document.getElementsByClassName("navigation")[0];
function openF() {
    var computedStyle = getComputedStyle(document.getElementsByClassName("navigation")[0]);
    if (open == true) {
        navigation.style.display = "flex";
        ulnav.style.display = "flex";
    open = false;
    }
    else {
    if (computedStyle.position == "absolute") {
        navigation.style.display = "none";
    }
        ulnav.style.display = "none";
        open = true;
    }
}
document.getElementById("toggleId").addEventListener("click", openF);

var elements = $('.modal-overlay, .modal');

$('#buttonModal').click(function () {
    var computedStyle = getComputedStyle(document.getElementsByClassName("navigation")[0]);
    elements.addClass('active');
    if (open == false) {
    if (computedStyle.position == "absolute") {
        navigation.style.display = "none";
    }
        ulnav.style.display = "none";
        open = true;
    }
});

var elements = $('.modal-overlay, .modal');

$('button').click(function () {
    elements.addClass('active');
});

$('.close-modal').click(function () {
    elements.removeClass('active');
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