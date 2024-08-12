// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function mostrarOcultar() {
    let layout = document.getElementById('layout');
    let capa = document.getElementById('capaDeFondo');
    let layoutStyle = window.getComputedStyle(layout);
   // let capaStyle = window.getComputedStyle(capa);


    if (layoutStyle.getPropertyValue('left') == "-200px") {
        capa.style.setProperty('opacity', '0.5')
        capa.style.setProperty('z-index', '0');
        layout.style.setProperty('left', '0px');

    } else {
        layout.style.setProperty('left', '-200px');
        capa.style.setProperty('opacity', '0')
        capa.style.setProperty('z-index', '-10');

    }
     
}

////Saco y agrego clase en notificacion para que se abran a la derecha y no hacia abajo. No funciona aun
function toggleDropendClass() {
    var containerNoti = document.querySelector('.containerNoti');

    if (window.innerWidth <= 768) {
        containerNoti.classList.add('dropend');
        containerNoti.classList.remove('dropdown');
    } else {
        containerNoti.classList.remove('dropend');
        containerNoti.classList.add('dropdown');
    }
}

toggleDropendClass();

window.addEventListener('resize', toggleDropendClass);