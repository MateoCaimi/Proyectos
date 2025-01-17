import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    imagenes: [],
}

export const imagenesSlice = createSlice({
    name: "Imagen",
    initialState,
    reducers: {
        guardarImagen: (state, action) => {
            state.imagenes.push(action.payload)
        }
    }
})

export const { guardarImagen } = imagenesSlice.actions;

export default imagenesSlice.reducer;