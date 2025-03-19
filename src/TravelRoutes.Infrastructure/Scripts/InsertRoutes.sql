-- Inserindo rotas entre cidades brasileiras
INSERT INTO [Routes] ([Origin], [Destination], [Cost]) VALUES ('GRU', 'BRC', 10);
INSERT INTO [Routes] ([Origin], [Destination], [Cost]) VALUES ('BRC', 'SCL', 5);
INSERT INTO [Routes] ([Origin], [Destination], [Cost]) VALUES ('GRU', 'CDG', 75);
INSERT INTO [Routes] ([Origin], [Destination], [Cost]) VALUES ('GRU', 'SCL', 20);
INSERT INTO [Routes] ([Origin], [Destination], [Cost]) VALUES ('GRU', 'ORL', 56);
INSERT INTO [Routes] ([Origin], [Destination], [Cost]) VALUES ('ORL', 'CDG', 5);
INSERT INTO [Routes] ([Origin], [Destination], [Cost]) VALUES ('SCL', 'ORL', 20);

-- Legenda dos aeroportos:
-- GRU: Guarulhos, São Paulo
-- BRC: San Carlos de Bariloche, Argentina
-- SCL: Santiago, Chile
-- CDG: Charles de Gaulle, Paris
-- ORL: Orlando, Estados Unidos 