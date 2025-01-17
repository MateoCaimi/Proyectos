import React, { useEffect } from 'react'
import Categorias from "./Categorias";
import style from './css/Principal.module.css'



export const Principal = () => {
    
    
    console.log();

    return (
        <section className={style.section}>
            <h1 className={style.h1}>MAQUINARIA</h1>
            <p className={style.p}>Nueva y Usada</p>
            <Categorias />
        </section>
    )
}
