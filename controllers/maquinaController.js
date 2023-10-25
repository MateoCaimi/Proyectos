var express = require("express");
const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");

exports.maquinas_getAll = asyncHandler(async (req, res, next) => {
  const result = await dataBase.getMaquinas();
  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json(result);
  }
});

exports.maquina_get = asyncHandler(async (req, res, next) => {
  try {
    await dataBase.conectar();
    const maquina = await dataBase.ejecutarConsulta(
      `SELECT * FROM potencia_vial.Maquina where Id = ${req.params.id}`
    );
    const multimediasMaquinas = await dataBase.ejecutarConsulta(
      `SELECT * FROM potencia_vial.MaquinaMultimedia where MaquinaId = ${maquina[0].Id}`
    );
    const multimedias = await dataBase.ejecutarConsulta(
      `SELECT * from potencia_vial.Multimedia`
    );
    await dataBase.desconectar();

    const multimediaMap = new Map();
    for (const multimediaMaquina of multimediasMaquinas) {
      if (!multimediaMap.has(multimediaMaquina.MaquinaId)) {
        multimediaMap.set(multimediaMaquina.MaquinaId, []);
      }
      multimediaMap
        .get(multimediaMaquina.MaquinaId)
        .push(multimediaMaquina.MultimediaId);
    }

    const result = {
      Id: maquina[0].Id,
      Modelo: maquina[0].Modelo,
      TipoId: maquina[0].TipoId,
      Condicion: maquina[0].Condicion,
      Altura: maquina[0].Altura,
      Largo: maquina[0].Largo,
      Ancho: maquina[0].Ancho,
      CargaMaxima: maquina[0].CargaMaxima,
      MaquinaMultimedia: (multimediaMap.get(maquina[0].Id) || []).map(
        (multimediaId) => {
          const multimedia = multimedias.find(
            (item) => item.Id === multimediaId
          );
          return multimedia; // Agregar el elemento multimedia correspondiente a la lista
        }
      ),
    };
    if (!result) {
      throw new Error("No se encontró la máquina.");
    } else {
      res.status(200).json(result);
    }
  } catch (error) {
    next(error);
  }
});

exports.maquinasAndType = asyncHandler(async (req, res, next) => {
  // Obtener todos los tipos, máquinas, multimedias de máquinas y multimedias de manera eficiente
  // const [multimediasMaquinas, multimedias] = await Promise.all([
  //   dataBase.getTipos(),
  //   dataBase.getMaquinasActivas(),
  //   dataBase.getMultimediaByMaquina(),
  //   dataBase.getMultimedia(),
  // ]);

  await dataBase.conectar();
  const tipos = await dataBase.ejecutarConsulta(
    `SELECT * from potencia_vial.Tipo`
  );
  const maquinas = await dataBase.ejecutarConsulta(
    `SELECT * from potencia_vial.Maquina`
  );
  const multimediasMaquinas = await dataBase.ejecutarConsulta(
    `SELECT * from potencia_vial.MaquinaMultimedia`
  );
  const multimedias = await dataBase.ejecutarConsulta(
    `SELECT * from potencia_vial.Multimedia`
  );
  await dataBase.desconectar();
  // Crear un mapa para relacionar multimedia con máquinas
  const multimediaMap = new Map();
  for (const multimediaMaquina of multimediasMaquinas) {
    if (!multimediaMap.has(multimediaMaquina.MaquinaId)) {
      multimediaMap.set(multimediaMaquina.MaquinaId, []);
    }
    multimediaMap
      .get(multimediaMaquina.MaquinaId)
      .push(multimediaMaquina.MultimediaId);
  }

  // Crear la estructura de datos resultante
  const result = {
    Tipos: tipos.map((tipo) => ({
      Id: tipo.Id,
      Nombre: tipo.Nombre,
      Descripcion: tipo.Descripcion,
      Maquinas: maquinas
        .filter((maquina) => maquina.TipoId === tipo.Id)
        .map((maquina) => ({
          Id: maquina.Id,
          Modelo: maquina.Modelo,
          TipoId: maquina.TipoId,
          Condicion: maquina.Condicion,
          Altura: maquina.Altura,
          Largo: maquina.Largo,
          Ancho: maquina.Ancho,
          CargaMaxima: maquina.CargaMaxima,
          MaquinaMultimedia: (multimediaMap.get(maquina.Id) || []).map(
            (multimediaId) => {
              const multimedia = multimedias.find(
                (item) => item.Id === multimediaId
              );
              return multimedia; // Agregar el elemento multimedia correspondiente a la lista
            }
          ),
        })),
    })),
  };

  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json({ result });
  }
});
