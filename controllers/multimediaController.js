const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");
const fs = require("fs");
const path = require("path");

exports.getMultimedia = asyncHandler(async (req, res, next) => {
  const result = await dataBase.get_Multimedia(req.params.id);
  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json(result);
  }
});

exports.upload = asyncHandler(async (req, res, next) => {
  try {
    const MaquinaId = req.body.MaquinaId;

    const { filename } = req.file;
    const fileType = req.file.mimetype;
    const filePath = path.join(__dirname, "../public/images", filename);

    const query =
      "INSERT INTO potencia_vial.Multimedia (Tipo, Nombre, Valor) VALUES (?, ?, ?)";
    const imageContentBuffer = fs.readFileSync(filePath);
    const imageContent = imageContentBuffer.toString("base64");
    const result = await dataBase.insertarMultimedia(
      query,
      filename,
      fileType,
      imageContent
    );

    const MultimediaId = result.insertId;

    const data = {
      MaquinaId,
      MultimediaId,
    };

    await dataBase.insertarMaquinaMultimedia(data);

    if (!result) {
      throw new Error("No se encontró la máquina.");
    } else {
      res.status(200).json(result);
    }
  } catch (error) {
    console.log(error);
    next(error);
  }
});
