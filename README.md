# Sistema Editoras — .NET MAUI (.NET 9)

Projeto da disciplina **Computação para Dispositivos Móveis**: cadastro de **Editoras**, **Autores** e **Livros**
com banco local **SQLite**, navegação **Shell (Flyout + TabBar)** e CRUD completo de cada entidade.

- Visual Studio 2022 (17.12 ou superior) com a carga de trabalho **.NET Multi-platform App UI development**
- **.NET 9 SDK** · alvo `net9.0-android` · testado em emulador Android
- Pacotes: `sqlite-net-pcl` + `SQLitePCLRaw.bundle_green`

---

## 1. Conectar o GitHub ao Visual Studio

### 1.1 Entrar com a conta do GitHub
1. No Visual Studio, clique no ícone da sua conta (canto superior direito) → **Adicionar outra conta…** → **GitHub**
   (ou **Arquivo → Configurações de Conta → Adicionar → GitHub**).
2. Autorize no navegador. Pronto: o Visual Studio passa a enviar/receber do GitHub sem pedir senha.

### 1.2 Clonar este repositório
1. Tela inicial → **Clonar um repositório** (ou menu **Git → Clonar Repositório…**).
2. Em *Local do repositório* cole:

   ```
   https://github.com/Tavimmm21/SistemasEditoras2.git
   ```
3. Escolha uma pasta **curta** (ex.: `C:\Projetos`) e clique em **Clonar**.

   > Dica: caminhos muito longos causam erro de build no Android. Evite pastas profundas.

### 1.3 Ir para a branch de trabalho
O código foi enviado na branch `claude/trusting-maxwell-gdqkcu`.

1. Menu **Git → Gerenciar Branches** (ou clique no nome da branch na barra inferior).
2. Expanda `origin` e dê **duplo clique** em `claude/trusting-maxwell-gdqkcu`.

### 1.4 Abrir e rodar
1. Abra `SistemasEditoras.sln` (o Visual Studio restaura os pacotes NuGet sozinho).
2. Na barra superior escolha um **Android Emulator** como destino.
3. Use **▶ Executar (F5)** — *Recompilar* só compila, não abre o app no emulador.

### 1.5 Subir suas alterações (dia a dia)
1. **Exibir → Alterações do Git**.
2. Escreva a mensagem do commit → **Confirmar Tudo**.
3. Clique em **Efetuar Push** (ou use **Confirmar Tudo e Efetuar Push**).
4. Para trazer novidades do GitHub: **Efetuar Pull**.

Pelo terminal (equivalente):

```bash
git clone https://github.com/Tavimmm21/SistemasEditoras2.git
cd SistemasEditoras2
git checkout claude/trusting-maxwell-gdqkcu
```

```bash
git add -A
git commit -m "descreva a alteracao"
git push
```

### 1.6 Boas práticas do plano de ação
- Trabalhe em **branch separada da `main`**; só faça merge na `main` depois de testar no emulador.
- Uma etapa por vez (navegação → banco → CRUD de cada entidade), testando antes de avançar.

---

## 2. Estrutura do projeto

```
SistemasEditoras.sln
SistemasEditoras/
├── AppShell.xaml          Flyout + TabBar (um FlyoutItem com uma Tab por seção)
├── MainPage.xaml          Home com botões ícone + texto
├── MauiProgram.cs         Injeção de dependência (DatabaseService, repositórios, páginas)
├── Models/                Editora, Autor, Livro (atributos sqlite-net = diagrama)
├── Dados/                 DatabaseService + 1 repositório por entidade
├── Helpers/Validacao.cs   Validação de ano e ISBN
├── Views/                 3 telas por entidade: Principal / Inserir / Alterar
│                          + DadosDesenvolvedor
└── Platforms/Android/     Arquivos específicos do Android
```

## 3. Banco de dados (arquivo `editoras.db3`)

| Tabela | Colunas |
|---|---|
| `tbleditoras` | `edid` (PK), `edinome` (50), `edisigla` (25), `edobservacoes` (500) |
| `tblautores` | `autid` (PK), `autnome` (50), `aupseudonimo` (50), `autobservacoes` (500) |
| `tbllivros` | `livid` (PK), `livnome` (50), `livanopublicacao`, `livisbn` (decimal 14), `livobservacoes` (500), `edid` (FK), `autid` (FK) |

Regras de negócio implementadas:
- Campos obrigatórios validados antes de salvar (`DisplayAlert` bloqueia).
- **Livro** só pode ser cadastrado se já existir ao menos uma **editora** e um **autor**.
- Confirmação (Sim/Não) antes de qualquer exclusão.
- Não é possível excluir uma editora/autor que ainda tenha livros vinculados.

## 4. Antes de gravar o vídeo

1. **Preencha seus dados** em `Views/DadosDesenvolvedor.xaml.cs` (nome, RA e data da avaliação).
2. **Zere o banco**: desinstale o app do emulador (ou limpe os dados dele) — o app deve abrir sem nenhum registro.
3. Confira o enunciado da prova: o roteiro do vídeo pode variar.
4. Grave **só a tela do emulador** (nunca o código-fonte), com link público testado em aba anônima.

> Se você alterou o modelo de dados (ex.: novo campo) e o app já estava instalado, **desinstale o app do emulador**
> para o banco ser recriado com a nova estrutura.

## 5. Plano B: versão do SDK diferente

Confira os SDKs instalados no terminal:

```bash
dotnet --list-sdks
```

Se não houver o .NET 9, troque **somente** a linha `TargetFrameworks` em `SistemasEditoras/SistemasEditoras.csproj`
(ex.: `net8.0-android`).

## 6. Entrega do código-fonte

1. Feche o Visual Studio.
2. Botão direito na **Solução → Limpar** (não confundir com *Limpeza de Código*).
3. Se `bin`/`obj` travarem, copie a pasta para outro lugar e apague dentro da cópia.
4. Compacte em `.zip` **sem** `.git`, `.vs`, `bin` e `obj`.
