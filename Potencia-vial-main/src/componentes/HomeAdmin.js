import { useRef } from "react"

const HomeAdmin = () => {

const nombreCampo = useRef(null)

const Buscar= ()=>{
const nombreActual = nombreCampo.current.value;
//LLamar a la api devolver lista 
}

  return (
    <div>
        <label htmlFor="buscar">Buscar por nombre:</label>
        <input type="text" id="buscar" ref={nombreCampo}></input>
        <input type="button" value='Buscar' onClick={Buscar}></input>
    </div>
  )
}

export default HomeAdmin