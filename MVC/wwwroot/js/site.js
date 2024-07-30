// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function mostrarOcultar() {
    let layout = document.getElementById('layout');
    let capa = document.getElementById('capaDeFondo');
    let layoutStyle = window.getComputedStyle(layout);
   // let capaStyle = window.getComputedStyle(capa);


    if (layoutStyle.getPropertyValue('left') == "-250px") {
        capa.style.setProperty('opacity', '0.5')
        capa.style.setProperty('z-index', '0');
        layout.style.setProperty('left', '0px');

    } else {
        layout.style.setProperty('left', '-250px');
        capa.style.setProperty('opacity', '0')
        capa.style.setProperty('z-index', '-10');

    }
     
}