import './App.css';
import './bootstrap.min.css';
import Administrador from './componentes/Administrador';
import Contenedor from './componentes/Contenedor';
import Home from './componentes/Home';
import { store } from './store/store';
import { Provider } from 'react-redux';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

function App() {
    return(
    <Provider store={store} >
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Contenedor />}>

                    <Route path='/' element={<Home/>}/>
                    <Route path="/administrador" element={<Administrador/>} /> 

                </Route>

            </Routes>
        </BrowserRouter>
    </Provider>
    )
}
export default App;
