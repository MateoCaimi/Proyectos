import ImgTel from '../imagenes/001-llamada-telefonica.png'
import ImgHome from '../imagenes/004-casa.png'
import ImgWeb from '../imagenes/003-navegador.png'
import ImgMail from '../imagenes/002-email.png'
import logo from '../imagenes/logoultimo.png'
import style from './css/Footer.module.css'



const Footer = () => {
    return (
        <footer className={style.contenedor}>
            <figure className={style.figure}>
                <img width='300px' src={logo}></img>
            </figure>

            <ul className={style.ul}>
                <li className={style.li}>
                    <img src={ImgTel}></img>
                    <span>098683688</span>
                </li>
                <li className={style.li}>
                    <img src={ImgMail}></img>
                    <span>potenciavial@gmail.com </span>
                </li>
                <li className={style.li}>
                    <img src={ImgHome}></img>
                    <span>Ruta 11 Km 162.500, Atlantida </span>
                </li>
                <li className={style.li}>
                    <img src={ImgHome}></img>
                    <span>Zenon, Burgueño. Soca </span>
                </li>
                <li className={style.li}>
                    <img src={ImgWeb}></img>
                    <span>https://www.potenciavial.com.uy/</span>
                </li>
            </ul>
        </footer>
        )
}

export default Footer