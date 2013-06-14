$(document).ready(function () {
  
    $("#adminbar").find(".sub_menu").hide();
    $("#adminbar").find(".menupop").hover(
    function () {
        $(this).find("ul").show();
        $(this).find(".first_menu").find("a").css("color", "black")
        $(this).css({ "background-color":"white" });
    },
    function () {
        $(this).find("ul").hide();
        $(this).find(".first_menu").find("a").css("color", "rgb(204, 204, 204)");
        $(this).css("background-color", "rgb(70, 70, 70)");
    }
);
});

