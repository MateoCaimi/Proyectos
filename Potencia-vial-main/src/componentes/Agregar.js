import React, { useRef } from 'react'

const Agregar = () => {

    const nombreCampo = useRef(null)
    const tipoCampo = useRef(null)
    const marcaCampo = useRef(null)
    const modeloCampo = useRef(null)
    const anioCampo = useRef(null)
    const descripcionCampo = useRef(null)
    const condicionCampo = useRef(null)

    const agregar= (event)=>{
        event.preventDefault()

        let nombre = nombreCampo.current.value
        let tipo = tipoCampo.current.value
        let marca = marcaCampo.current.value
        let modelo = modeloCampo.current.value
        let anio = anioCampo.current.value
        let descripcion = descripcionCampo.current.value
        let condicion = condicionCampo.current.value

        //Llamara a la api
    }

    return (
        <div className="m-2">
            <h3 className="text-warning">Agregar Maquina</h3>
            <form onSubmit={agregar}>
                <div className="mb-3">
                    <label htmlFor="nombreAgregar" className="form-label">Nombre: </label>
                    <input type="text" className="form-control" id="nombreAgregar" ref={nombreCampo} required />
                </div>
                <div>
                    <label htmlFor="tipoAgregar">Tipo: </label>
                    <select required className="form-select" id="tipoAgregar" ref={tipoCampo}>
                        <option hidden defaultValue={""}>Seleccionar</option>
                        {dptos.map(d => <option key={d.id} value={d.id}>{d.nombre}</option>)}
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="marcaAgregar" className="form-label">Marca: </label>
                    <input type="text" className="form-control" id="marcaAgregar" ref={marcaCampo} required />
                </div>
                <div className="mb-3">
                    <label htmlFor="modeloAgregar" className="form-label">Modelo: </label>
                    <input type="text" className="form-control" id="modeloAgregar" ref={modeloCampo} required />
                </div>
                <div className="mb-3">
                    <label htmlFor="anioAgregar" className="form-label">Año: </label>
                    <input type="text" className="form-control" id="anioAgregar" ref={anioCampo} required />
                </div>
                <div className="mb-3">
                    <label htmlFor="descripcionAgregar" className="form-label">Descripcion: </label>
                    <input type="text" className="form-control" id="descripcionAgregar" ref={descripcionCampo} required />
                </div>
                <div>
                    <label htmlFor="condicionAgregar">Condicion: </label>
                    <select required className="form-select" id="condicionAgregar" ref={condicionCampo}>
                        <option hidden defaultValue={""}>Seleccionar</option>
                        <option value='nuevo'>Nuevo</option>
                        <option value='usado'>Usado</option>
                    </select>
                </div>
                <button className="btn btn-primary mt-3" type="submit" >Agregar</button>
            </form>
            {error && <div className="text-danger">{error}</div>}

        </div>
    )
}

export default Agregar