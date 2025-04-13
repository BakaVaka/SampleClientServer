using System.Collections.Concurrent;

namespace Shared;

public static class NameGenerator {

    private static readonly ConcurrentDictionary<string, int> _names = [];

    private static string[] Parts1 = [
            "Joyful", "Radiant", "Vibrant", "Gentle", "Elegant", "Charming",
            "Playful", "Enthusiastic", "Creative", "Inventive", "Resourceful", "Curious",
            "Adventurous", "Optimistic", "Confident", "Sincere", "Compassionate", "Loyal", "Honest",
            "Generous", "Gracious", "Polite", "Humble", "Patient", "Calm", "Peaceful",
            "Harmonious", "Balanced", "Refreshing", "Inspiring", "Motivating", "Empowering"
        ];
    
    private static readonly string[] Parts2 = [
            "Aardvark", "Albatross", "Alligator", "Antelope", "Armadillo",
            "Baboon", "Badger", "Bat", "Bear", "Beaver",
            "Bison", "Boar", "Buffalo", "Butterfly", "Camel",
            "Capybara", "Caribou", "Cassowary", "Cat", "Caterpillar",
            "Cattle", "Chamois", "Cheetah", "Chicken", "Chimpanzee",
            "Chinchilla", "Chough", "Clam", "Cobra", "Cockroach",
            "Cod", "Cormorant", "Coyote", "Crab", "Crane",
            "Crocodile", "Crow", "Curlew", "Deer", "Dinosaur",
            "Dog", "Dogfish", "Dolphin", "Donkey", "Dotterel",
            "Dove", "Dragonfly", "Duck", "Dugong",
            "Dunlin", "Eagle", "Echidna", "Eel", "Eland",
            "Elephant", "Elk", "Emu", "Falcon", "Ferret",
            "Finch", "Fish", "Flamingo", "Fly", "Fox", "Frog", 
            "Gaur", "Gazelle", "Gerbil", "Giraffe", "Gnat", "Gnu",
            "Goat", "Goldfish", "Goose", "Gorilla", "Goshawk",
            "Grasshopper", "Grouse", "Guanaco", "Gull", "Hamster",
            "Hare", "Hawk", "Hedgehog", "Heron", "Herring",
            "Hippopotamus", "Hornet", "Horse", "Human", "Hummingbird",
            "Hyena", "Ibex", "Ibis", "Jackal", "Jaguar", "Jay",
            "Jellyfish", "Kangaroo", "Kingfisher", "Koala", "Kookabura",
            "Kouprey", "Kudu", "Lapwing", "Lark", "Lemur",
            "Leopard", "Lion", "Llama", "Lobster", "Locust",
            "Loris", "Louse", "Lyrebird", "Magpie", "Mallard",
            "Manatee", "Mandrill", "Mantis", "Marten", "Meerkat",
            "Mink", "Mole", "Mongoose", "Monkey", "Moose",
            "Mosquito", "Mouse", "Mule", "Narwhal", "Newt",
            "Nightingale", "Octopus", "Okapi", "Opossum", "Oryx",
            "Ostrich", "Otter", "Owl", "Oyster", "Panther", 
            "Parrot", "Partridge", "Peafowl", "Pelican", "Penguin",
            "Pheasant", "Pig", "Pigeon", "Pony", "Porcupine",
            "Porpoise", "Quail", "Quelea", "Quetzal", "Rabbit",
            "Raccoon", "Rail", "Ram","Rat", "Raven", 
            "Red deer", "Red panda", "Reindeer", "Rhinoceros", "Rook",
            "Salamander", "Salmon", "Sand Dollar", "Sandpiper", "Sardine",
            "Scorpion", "Seahorse", "Seal", "Shark", "Sheep",
            "Shrew", "Skunk", "Snail", "Snake", "Sparrow",
            "Spider", "Spoonbill", "Squid", "Squirrel", "Starling",
            "Stingray", "Stinkbug", "Stork", "Swallow", "Swan",
            "Tapir", "Tarsier", "Termite", "Tiger", "Toad",
            "Trout", "Turkey", "Turtle", "Viper", "Vulture",
            "Wallaby", "Walrus", "Wasp", "Weasel", "Whale",
            "Wildcat", "Wolf", "Wolverine", "Wombat", "Woodcock", 
            "Woodpecker", "Worm", "Wren", "Yak", "Zebra"
        ];

    public static string Generate() {
        string adjective = Parts1[Random.Shared.Next(Parts1.Length)];
        string animal = Parts2[Random.Shared.Next(Parts2.Length)];
        string key = $"{adjective}-{animal}";

        int uniqueNumber = _names.AddOrUpdate(key, 1, (k, v) => v + 1);

        return $"{adjective} {animal} {uniqueNumber}";
    }
}
