$(document).ready(function () {
    $(".endsummon-btn").hide();
    $(".endsummon-btn").click(closeSummon);
    $(".pokemon-label").css("display", "none");


    // when user clicks on pokeball simulate summon
    $(".pokeball").click(() => {
        $(".pokeball").css("display", "none");
        $(".summon-pokemon").css("display", "inline");
        $(".summon-pokemon").css("animation", "summon 2s linear");
        $(".summon-pokemon").attr("src", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png");
        $(".pokemon-label").text("The Pokemon name goes here" + "!");
        $(".pokemon-label").css("display", "block");

        //run when summon animation ends
        $(".summon-pokemon").one("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd",
            function () {
                $(".endsummon-btn").show();
            });
    });

    //hides summon display material
    function closeSummon() {
        $(".summon-pokemon").css("display", "none");
        $(".pokeball").css("display", "block");
        $(".endsummon-btn").hide();
        $(".pokemon-label").css("display", "none");



    }
});