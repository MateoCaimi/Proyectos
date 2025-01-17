import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    tipos:[]
}

export const tiposSlice = createSlice({
    name: "Tipos",
    initialState,
    reducers: {
        guardarTipos: (state, action) => {
            state.tipos = action.payload
        }
        
      
    }
})

export const { guardarTipos } = tiposSlice.actions;

export default tiposSlice.reducer;