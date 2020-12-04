$(document).ready(function () {
    $(".endsummon-btn").hide();
    $(".endsummon-btn").click(closeSummon);
    $(".pokemon-label").css("display", "none");


    // when user clicks on pokeball simulate summon
    $(".pokeball").click(() => {
        $(".pokeball").css("display", "none");
        $("#loading-text").text("Loading...");
        
        updatePokemonCollection().then(() => {
            getPokemon().then((value) => {
                addPokemon();
                $("#loading-text").text("");
                $(".summon-pokemon").css("animation", "summon 1.5s linear");
                $(".summon-pokemon").attr("src", rolledPokemon.spriteUrl);
                $(".summon-pokemon").css("display", "inline");
                //run when summon animation ends
                $(".pokemon-label").css("display", "inline-block");
                $(".pokemon-label").text(rolledPokemon.name + "!");
                $(".summon-pokemon").one("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd",
                    function () {
                        //$(".pokemon-result").css("background", "rgba(255, 255, 255, 0.8)");
                        $(".endsummon-btn").show();
                    }
                );
            })
        });
    });

    //hides summon display material
    function closeSummon() {
        $(".summon-pokemon").css("display", "none");
        $(".pokeball").css("display", "block");
        $(".endsummon-btn").hide();
        $(".pokemon-label").css("display", "none");
        //$(".pokemon-result").css("background", "none");
    }
});