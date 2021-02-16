const [nameInput, imageInput] = Array.from(document.querySelectorAll('input[type="text"]'));
const [attackInput, defenseInput] = Array.from(document.querySelectorAll('input[type="number"]'));
const descriptionTextArea = document.getElementsByTagName('textarea')[0];
const nameElement = document.getElementById('cardName');

nameInput.addEventListener('input', () => {
    const name = nameInput.value;
    nameElement.textContent = name;
});

const imageElement = document.getElementById('cardImage');
imageInput.addEventListener('change', () => {
    imageElement.src = imageInput.value;
});

const descriptionElement = document.getElementById('cardDescription');
descriptionTextArea.addEventListener('input', () => {
    descriptionElement.textContent = descriptionTextArea.value;
});

const statsElement = document.getElementById('cardStats');
attackInput.addEventListener('input', () => {
    const attack = attackInput.value;
    const defense = statsElement.textContent.split('DEF: ')[1];

    statsElement.textContent = `ATT: ${attack} | DEF: ${defense}`;
});

defenseInput.addEventListener('input', () => {
    const defense = defenseInput.value;
    const attackStats = statsElement.textContent.split(" | ")[0];
    const attackParts = attackStats.split(' ').filter(x => x !== '');
    const attack = attackParts[attackParts.length - 1];

    statsElement.textContent = `ATT: ${attack} | DEF: ${defense}`;
});

window.onload = function () {
    const channel = sessionStorage.getItem('selectedChannel');
    document.getElementById('channel').value = channel;
}