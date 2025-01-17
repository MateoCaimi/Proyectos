import React, { useRef } from 'react'

const Administrador = () => {

    const user = useRef(null)
    const pass = useRef(null)
  

    const ingresar = () => {
        const userCampo = user.current.value;
        const passCampo = pass.current.value;

        //Llamar ala api
    }

    return (
        <div>
            <h2>Ingresar</h2>
            <form className="">
                <div className="mb-3">
                    <label htmlFor="usuario" className="form-label">Usuario: </label>
                    <input type="text" className="form-control" id="usuario" ref={user}/>
                </div>
                <div className="mb-3">
                    <label htmlFor="pass" className="form-label">Contraseña: </label>
                    <input type="password" className="form-control" id="pass" ref={pass}/>
                </div>
                <input className="btn btn-outline-primary" type="button" value="Ingresar" onClick={ingresar} />
                {/* {error && <p>{error}</p>} */}
            </form>
        </div>
    )
}

export default Administrador