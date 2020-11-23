$(document).ready(function () {

    $(".pokeball").click(() => {
        console.log("hello");
        $(".pokeball").css("display", "none");
        $(".summon-pokemon").css("display", "inline");
        $(".summon-pokemon").css("animation", "summon 2s linear");
        $(".summon-pokemon").attr("src", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png");

        $(".summon-pokemon").one("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd",
            function () {
                console.log("worked");
                $(".summon-pokemon").css("display", "none");
                $(".pokeball").css("display", "block");
            });
    });
});