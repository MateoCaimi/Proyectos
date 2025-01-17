import style from './css/Busqueda.module.css'
import { useRef } from 'react'

const Busqueda = () => {

    const cat = useRef(null)
    const marca = useRef(null)
    const nuevo = useRef(null)
  

    const Buscar = () => {
        const catCampo = cat.current.value;
        const marcaCampo = marca.current.value;
        const nuevoCampo = nuevo.current.value;

        //Llamar ala api
    }

    return (
        <section className={style.section}>
            <form className={style.form}>
                <label className={style.label} htmlFor="categorias"></label>
                <select className={style.select} id="categorias" ref={cat}>
                    <option hidden defaultValue={""}>Categorias</option>
                    <option value={"todas"}>Todas</option>
                </select>

                <label htmlFor="marca" className={style.label}></label>
                <select className={style.select} id="marca" ref={marca}>
                    <option hidden defaultValue={""}>Marcas</option>
                    <option value={"todas"}>Todas</option>
                </select>

                <label className={style.label} htmlFor="nuevo">Nuevo</label>
                <input type="checkbox"  className={style.input} id="nuevo" ref={nuevo}/>
            </form>
            <input className={style.button} value='Buscar' type='button' onClick={Buscar}/>
        </section>
    )
}

export default Busqueda