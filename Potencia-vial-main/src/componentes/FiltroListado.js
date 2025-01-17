
const FiltroListado = () => {

    

    return (
        <div>
           <h3>Filtro</h3>
           <h5>Nuevo</h5>
           <label className={style.label} htmlFor="nuevo">Nuevo</label>
            <input type="checkbox"  className={style.input} id="nuevo" ref={nuevo}/>
            <h5>Marcas</h5>
        </div>
    )
}

export default FiltroListado