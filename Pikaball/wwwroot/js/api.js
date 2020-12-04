//This file retrieves pokemon data from the api to a json in a loop of requests 

//holds and array of json objects
let collectedPokemon;

//this is the resulting pokemon json that will be fed to the dbcontext
let rolledPokemon;

//this lock restricts the player access to mythical rarity pokemons
let mythicalLocked = true;

//this lock restricts the player access to legendary rarity pokemons
let legendaryLocked = true;

//Max pokemon id number that can be summoned, restricted to kanto region pokemon
const MAXPOKEMONID = 151;

//The default minimum level requirements to evovle for pokemon with no leveling requirements
const DEFAULTLEVEL = 30;

//variables required to run the pokemon draw.
let pokemonID;
let result;

//this is to check if player can draw the pokemon such as, if they are
//allowed to get the legendary if they draw one, if not, redraw in the getPokemon()
function isMythLegendCheck() {
    if (result.isMythical && !mythicalLocked) {
        mythicalLocked = true;
        return 1;
    }
    if (result.isLegendary && !legendaryLocked) {
        legendaryLocked = true;
        return 1;
    }
    if (!result.isMythical && !result.isLegendary) {
        return 1;
    }
    return 0;
}

/*
 *  will continue rolling pokemon until the draw is
 *  something that the player can obtain.
 * 
 * Also increments the existing pokemon's level if duplicate is obtained.
 * This also checks if the pokemon meets the requirement to evolve and sets the new condition.
 */
async function getPokemon() {
    unlockLegendaryMythical();
    while (1) {
        pokemonID = Math.floor(Math.random() * MAXPOKEMONID) + 1;
        console.log(pokemonID);
        if (pokemonExists(pokemonID)) {
            if (isMythLegendCheck(result)) {
                console.log("duplicate");
                if (result.level < 100) {
                    result.level += 1;
                }
                if (result.evolutionCondition == result.level) {
                    result.EvolutionUnlocked = true;
                }
                break;
            }
        } else {
            result = await fetchPokemonData(pokemonID);

            if (pokemonExists(pokemonID)) {
                if (isMythLegendCheck(result)) {
                    console.log("duplicate");
                    if (result.level < 100) {
                        result.level += 1;
                    }
                    if (result.evolutionCondition == result.level) {
                        result.EvolutionUnlocked = true;
                    }
                    break;
                }
            }
            if (isMythLegendCheck(result)) {
                result.PokedexID = pokemonID;
                result.level = 1;
                result.EvolutionUnlocked = false;
                break;
            }
        }
        result = null;
    }
    rolledPokemon = result;
}

//this function will unlock so that the user can get the rare pokemon once they draw it
//legendary 1% chance
//mythical 10% chance
function unlockLegendaryMythical() {
    let legendary = Math.floor(Math.random() * 1000);
    let mythical = Math.floor(Math.random() * 1000);
    if (legendary < 10) {
        legendaryLocked = false;
    }
    if (mythical < 100) {
        mythicalLocked = false;
    }
}

//check through the exisiting collection of pokemon to see if the user has the pokemon
function pokemonExists(id) {
    for (let i = 0; i < collectedPokemon.length; i++) {
        if (collectedPokemon[i].pokedexID == id) {
            result = collectedPokemon[i];
            return true;
        }
    }
    return false;
}

//check if the next evolution of the specified pokemon is unlocked
function pokemonUnlocked(id) {
    let checkCollection = pokemonExists(id);
    console.log(checkCollection);
    if (checkCollection && result.evolutionUnlocked) {
        return true;
    }
    return false;
}

//Fetches the correct pokemon given the pokemon ID and returns specific data in JSON
async function fetchPokemonData(id) {
    let pokeData2 = await fetchApi("https://pokeapi.co/api/v2/pokemon-species/", id);
    // If pokemon has a previous evolution fetch its api
    if (pokeData2.evolves_from_species) {
        let prevPokemon = await fetchApi(pokeData2.evolves_from_species.url);
        // checks if previous Pokemon's id exists in the region and it is not unlocked. If so fetch and return previous pokemon data instead.
        if ((prevPokemon.id <= MAXPOKEMONID) && (!pokemonUnlocked(prevPokemon.id))) {
            return fetchPokemonData(prevPokemon.id);
        }
    }
    //updates to correct pokemon id
    pokemonID = id;

    //primary pokemon api
    let pokeData1 = await fetchApi("https://pokeapi.co/api/v2/pokemon/", id);
    let evolutionData = await fetchApi(pokeData2.evolution_chain.url);
    let name = pokeData1.name;
    let description = fetchEnglishDescription(pokeData2.flavor_text_entries);
    description = description.replace(/\n|\f/g, ' ');
    let evolutionCondition = findEvolution(evolutionData.chain, name);
    let spriteUrl = pokeData1.sprites.front_default;
    let type1 = pokeData1.types[0].type.name;
    let isMythical = pokeData2.is_mythical;
    let isLegendary = pokeData2.is_legendary;
    if (pokeData1.types.length > 1) {
        let type2 = pokeData1.types[1].type.name;
        return {
            name: name, description: description, EvolutionCondition: evolutionCondition, spriteUrl: spriteUrl,
            isMythical: isMythical, isLegendary: isLegendary, Type1: type1, Type2: type2
        };
    } else {
        return {
            name: name, description: description, EvolutionCondition: evolutionCondition, spriteUrl: spriteUrl,
            isMythical: isMythical, isLegendary: isLegendary, Type1: type1
        };
    }
}

//Filters through the description and returns the english description
function fetchEnglishDescription(flavor_text_entries) {
    description = "none";
    for (i of flavor_text_entries) {
        if (i.language.name == "en") {
            return description = i.flavor_text;
        }
    }
    return "none"
}
//Given the parameters, returns the data in JSON including necessary data.
function convertToJson(name, description, evolutionCondition, spriteUrl, isMythical, isLegendary, type1, type2 = -1) {
    let pokeDataJson = {
        name: name, description: description, EvolutionCondition: evolutionCondition, spriteUrl: spriteUrl, isMythical: isMythical, isLegendary: isLegendary, Type1: type1
    };
    if (type2 != -1) {
        pokeDataJson["type2"] = type2;
    }
    return pokeDataJson;
}

//recursive, check if there is evolution.
//Searches and finds the correct pokemon evolution level in the chain
function findEvolution(evolutionChain, name) {
    // checks if pokemon name is same as given
    if (evolutionChain.species.name == name) {
        // checks if there is no next evolution 
        if (evolutionChain.evolves_to.length == 0) {
            return null;
        }
        //checks all next evolutions for minimum evolution level requirement
        for (i of evolutionChain.evolves_to) {
            if (i.evolution_details[0].min_level != null) {
                return i.evolution_details[0].min_level;
            }
        }
        //if minimum level requirement not found, return default level (ex. evolution stone)
        return DEFAULTLEVEL;
    } else {
        for (i of evolutionChain.evolves_to) {
            let minimum_level = findEvolution(i, name);
            if (minimum_level != -1) {
                return minimum_level;
            }
        }
    }
    //if somehow neither a next evolution or empty next evolution array does not exist
    return -1;
}

//fetches the pokemon data given the api url and pokemon id. Param id is optional.
function fetchApi(url, id = -1) {
    //if it contains id
    if (id != -1) {
        return new Promise(function (resolve, reject) {
            fetch(url + "" + id)
                .then(response => response.json())
                .then(function (pokeData) {
                    resolve(pokeData);
                });
        });
        //if it does not contain id
    } else {
        return new Promise(function (resolve, reject) {
            fetch(url)
                .then(response => response.json())
                .then(function (pokeData) {
                    resolve(pokeData);

                });
        });
    }
}