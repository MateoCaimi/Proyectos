const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");
const fs = require("fs");
const path = require("path");

exports.upload = asyncHandler(async (req, res, next) => {
  try {
    const { filename } = req.file;
    const fileType = req.file.mimetype;

    console.log("fileName: " + filename);
    console.log("fileType: " + fileType);
    const filePath = path.join(__dirname, "uploads", filename);
    console.log('Ruta del archivo: ' + filePath);

    const query =
      "INSERT INTO Multimedia (Tipo, Nombre, Valor) VALUES (?, ?, ?)";
    const imageContent = fs.readFileSync(filePath);
    console.log("imageConten: " + imageContent);
    const result = dataBase.insertarMultimedia(
      query,
      filename,
      fileType,
      imageContent
    );
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
