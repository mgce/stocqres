import Icon from 'react-native-vector-icons/Ionicons';

const iconColor = 'black';
const iconSize = 32;

const icons = {};

export function loadIcon(name) {
    return Icon.getImageSource(name, iconSize, iconColor);
}

export async function loadIcons(names) {
    const tasks = names.map((name) => loadIcon(name));
    const results = await Promise.all(tasks);
    const set = results.map((item, index) => ({ [names[index]]: item }));
    return Object.assign(icons, ...set);
}

export function getIcon(name) {
    return icons[name];
}

export default function setup() {
    var tasks = [
        // other async tasks/promises
        loadIcons(['ios-swap', 'ios-trending-up']),
    ];

    Promise.all(tasks).then(()=> console.log("tasks done."));
}