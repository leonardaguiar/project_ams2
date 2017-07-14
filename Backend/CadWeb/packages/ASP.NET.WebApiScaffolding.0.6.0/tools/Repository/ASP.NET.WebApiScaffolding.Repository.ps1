[T4Scaffolding.Scaffolder(Description = "Create a Repository for Model")][CmdletBinding()]
param(        
    [parameter(Position = 0, Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string]$DbContextType,
	[string]$Area,
	[switch]$NoChildItems = $false,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

# Ensure you've referenced System.Data.Entity
(Get-Project $Project).Object.References.Add("System.Data.Entity") | Out-Null

# Ensure a valid Model is provided
$foundModelType = Get-ProjectType $ModelType -Project $Project
if (!$foundModelType) { 
	return 
}

# Ensure Primary Key of provided Model is retrievable 
$primaryKey = Get-PrimaryKey $foundModelType.FullName -Project $Project -ErrorIfNotFound
if (!$primaryKey) { 
	return 
}

# Get DbContextType if not provided
if(!$DbContextType) { 
	$DbContextType = [System.Text.RegularExpressions.Regex]::Replace((Get-Project $Project).Name, "[^a-zA-Z0-9]", "") + "Context" 
}

# File path. The filename extension will be added based on the template's <#@ Output Extension="..." #> directive
$outputPath = Join-Path Repositories ($foundModelType.Name + "Repository") 

# Override the default path for the scaffolded file if $Area is provided
if ($Area) {
	$areaFolder = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaFolder -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	
	$outputPath = Join-Path $areaFolder $outputPath
}

# Attempt to generate DbContext if $NoChildItems is not flagged
if (!$NoChildItems) {
		$dbContextScaffolderResult = Scaffold DbContext -ModelType $ModelType -DbContextType $DbContextType -Area $Area -Project $Project -CodeLanguage $CodeLanguage
		$foundDbContextType = $dbContextScaffolderResult.DbContextType
		if (!$foundDbContextType) { 
			return 
		}
}

if (!$foundDbContextType) { 
	$foundDbContextType = Get-ProjectType $DbContextType -Project $Project 
}

if (!$foundDbContextType) { 
	return 
}

# Prepare all the parameter values to pass to the template, then invoke the template with those values
$repositoryName = $foundModelType.Name + "Repository"
$modelTypePluralized = Get-PluralizedWord $foundModelType.Name
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$modelTypeNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName)
$repositoryName = $foundModelType.Name + "Repository"
$repositoryNamespace = [T4Scaffolding.Namespaces]::Normalize($namespace + "." + [System.IO.Path]::GetDirectoryName($outputPath).Replace([System.IO.Path]::DirectorySeparatorChar, "."))
$relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $project)
if (!$relatedEntities) { 
	$relatedEntities = @() 
}

Add-ProjectItemViaTemplate $outputPath `
	-Template RepositoryTemplate `
	-Model @{ 
		ModelType = [MarshalByRefObject]$foundModelType;
		PrimaryKey = [string]$primaryKey;
		Namespace = $namespace; 
		RepositoryNamespace = $repositoryNamespace;
		ModelTypeNamespace = $modelTypeNamespace; 
		ModelTypePluralized = [string]$modelTypePluralized; 
		Repository = $repositoryName; 
		DbContextNamespace = $foundDbContextType.Namespace.FullName;
		DbContextType = [MarshalByRefObject]$foundDbContextType;
		RelatedEntities = $relatedEntities;
	} `
	-SuccessMessage "Added Repository output at {0}" `
	-TemplateFolders $TemplateFolders `
	-Project $Project `
	-CodeLanguage $CodeLanguage `
	-Force:$Force