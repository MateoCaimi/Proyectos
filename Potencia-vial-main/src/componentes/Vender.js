import ContactoVender from './ContactoVender'
import FotoVender from './FotoVender'
import style from './css/Vender.module.css'

const Vender = () => {
  return (
    <div className={style.contenedor}>

        <div className={style.contacto}>
          <ContactoVender  />
        </div>
        <div className={style.foto}>
          <FotoVender  />
        </div>
    </div>
  )
}

export default Vender