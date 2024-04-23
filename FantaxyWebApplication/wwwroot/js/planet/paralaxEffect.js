console.log('Script paralax loaded');

const navPlanet = document.getElementById('navPlanet');
const planetAvatar = document.getElementById('planetAvatar');
const navLink = document.getElementById('navLink');
const clikme = document.getElementById('clikme');

// Increase the size of the nav when the mouse hovers over it
navPlanet.addEventListener('mouseover', () => {
    navPlanet.style.height = '450px'; // Change the height to whatever you want
    planetAvatar.style.display = 'block';
    navLink.style.display = 'block';
    clikme.style.display = 'none';
});

// Reduce the size of the nav when the mouse leaves the nav area
navPlanet.addEventListener('mouseout', () => {
    navPlanet.style.height = '110px';
    planetAvatar.style.display = 'none';
    navLink.style.display = 'none';
    clikme.style.display = 'block';
});

// Reduce the size of the nav when the user clicks outside its area