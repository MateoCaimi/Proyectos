import Busqueda from "./Busqueda";
import Vender from "./Vender";
import { Principal } from './Principal';
import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { guardarImagen } from '../features/categoriasSlice';




const Home = () => {

  const dispatch = useDispatch()
    
    useEffect(() => {
      dispatch(guardarImagen('../imagenes/categorias/bulldozer.png'),('../imagenes/categorias/cargadoresFrontalesOruga.png'))
    }, [])

  return (
    <div>
      <Principal/>
      <Busqueda/>
      <Vender/>
    </div>
  )
}

export default Home