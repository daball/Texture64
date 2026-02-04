# .NET 10.0 Upgrade Plan for Texture64

**Generated**: 2026-02-04  
**Source Framework**: .NET Framework 4.8  
**Target Framework**: .NET 10.0-windows (LTS)  
**Solution**: Texture64.sln  
**Branch**: upgrade-to-NET10  
**Strategy**: All-At-Once Migration

---

## Executive Summary

This plan outlines the migration of the Texture64 Windows Forms application from .NET Framework 4.8 to .NET 10.0. Based on the assessment, this is a **Medium complexity** upgrade with one project and 3,959 lines of code.

### Migration Strategy: **All-At-Once**

**Rationale:**
- Single project solution (no complex dependencies)
- Small to medium codebase (~4K LOC)
- Windows Forms is well-supported in .NET 10
- Faster completion with single comprehensive test cycle
- Lower coordination overhead

### Key Metrics from Assessment

| Metric | Value | Impact |
|--------|-------|--------|
| Projects to upgrade | 1 | Low complexity |
| Total lines of code | 3,959 | Medium |
| Estimated LOC to modify | 3,233+ (81.7%) | High |
| API compatibility issues | 3,233 | High (mostly binary incompatibilities) |
| NuGet packages | 4 | All compatible |
| Security vulnerabilities | 0 | ✅ None |

### Timeline Estimate

| Phase | Duration | Notes |
|-------|----------|-------|
| Pre-migration setup | 0.5 days | SDK installation, validation |
| Project conversion | 0.5 days | SDK-style conversion |
| Package updates | 0.25 days | Remove unnecessary packages |
| Compilation fixes | 1-2 days | Address binary incompatibilities |
| Testing | 2-3 days | Full regression testing |
| Deployment setup | 0.5-1 days | Configure new deployment |
| **Total** | **4.75-7.25 days** | ~1-1.5 weeks |

---

## Dependency Analysis

### Project Structure

```
Texture64.sln
└── Texture64.csproj (net48 → net10.0-windows)
    - Type: Windows Forms Application
    - Dependencies: None
    - Dependants: None
    - Status: ⚙️ Classic (non-SDK-style)
```

### Migration Order

Since there's only one project, the migration is straightforward:

**Phase 1**: Texture64.csproj
- No dependency considerations
- Can be migrated in single phase
- All changes applied together

---

## Package Update Reference

### Packages to Remove

| Package | Current Version | Reason | Action |
|---------|----------------|--------|--------|
| System.Runtime.InteropServices.WindowsRuntime | 4.3.0 | Functionality included in .NET 10 framework | **Remove** |

### Packages to Keep

| Package | Current Version | Status | Action |
|---------|----------------|--------|--------|
| Microsoft.Windows.SDK.Contracts | 10.0.26100.7463 | ✅ Compatible | Keep (if WinRT APIs are used) |
| System.Runtime.WindowsRuntime | 4.6.0 | ✅ Compatible | Keep initially, test if needed |
| System.Runtime.WindowsRuntime.UI.Xaml | 4.6.0 | ✅ Compatible | Keep initially, test if needed |

**Note**: The WindowsRuntime packages may be unnecessary in .NET 10 but should be tested before removal.

---

## Breaking Changes Catalog

### Critical Breaking Changes

#### 1. Windows Forms Binary Incompatibilities (3,003 issues)

**Description**: Windows Forms types have binary breaking changes between .NET Framework and .NET 10. This is expected and handled by the compiler.

**Affected APIs** (Top occurrences):
- `System.Windows.Forms.Label` (296 occurrences)
- `System.Windows.Forms.NumericUpDown` (125 occurrences)
- `System.Windows.Forms.TableLayoutPanel` (84 occurrences)
- `System.Windows.Forms.Button` (71 occurrences)
- `System.Windows.Forms.Control` properties (Size, Location, Name, etc.)

**Impact**: Requires recompilation. Most code will work after rebuild.

**Resolution**:
1. Ensure project targets `net10.0-windows`
2. Recompile all forms and designer files
3. Test designer functionality in Visual Studio
4. Address any compiler errors specific to API changes

**Estimated Effort**: Low-Medium (mostly automated by compiler, some manual fixes expected)

---

#### 2. System.Drawing Source Incompatibilities (230 issues)

**Description**: System.Drawing APIs have source-level incompatibilities.

**Affected APIs**:
- `System.Drawing.Bitmap` (47 occurrences)
- `System.Drawing.ContentAlignment` (33 occurrences)
- `System.Drawing.Image` (18 occurrences)

**Impact**: May require code adjustments or additional using directives.

**Resolution**:
1. Add `System.Drawing.Common` package if needed (likely included with Windows Desktop)
2. Review image manipulation code in N64Graphics.cs
3. Update any type casts or property accesses as indicated by compiler

**Estimated Effort**: Low-Medium

---

#### 3. Legacy Configuration System (18 issues)

**Description**: App.config-based configuration has changed in .NET Core+.

**Impact**: Configuration reading may need updates if custom configuration sections are used.

**Resolution**:
1. Simple app.config files continue to work
2. For complex config, consider migrating to Microsoft.Extensions.Configuration
3. Add `System.Configuration.ConfigurationManager` NuGet package if needed

**Estimated Effort**: Low (likely no changes needed for basic app.config)

---

#### 4. Legacy Windows Forms Controls (11 issues)

**Description**: Old controls (StatusBar, ContextMenu, MainMenu) have been removed.

**Affected Controls**:
- StatusBar → Use StatusStrip
- ContextMenu → Use ContextMenuStrip
- MainMenu/MenuItem → Use MenuStrip/ToolStripMenuItem
- ToolBar → Use ToolStrip
- DataGrid → Use DataGridView

**Impact**: If any legacy controls are used, they must be replaced.

**Resolution**:
1. Search codebase for legacy control usage
2. Replace with modern equivalents
3. Update designer files
4. Test UI functionality

**Estimated Effort**: Low-Medium (depends on usage)

---

### Project-Specific Changes

#### Texture64.csproj

**Current State**:
- Target Framework: net48
- Project Format: Classic (non-SDK-style)
- Type: WinExe (Windows Forms)
- SDK-style: False

**Required Changes**:

1. **Convert to SDK-Style Project**
   - Remove old project format boilerplate
   - Use simplified SDK-style syntax
   - Update to .NET SDK project structure
   - **Estimated Effort**: 1-2 hours

2. **Update Target Framework**
   ```xml
   <TargetFramework>net10.0-windows</TargetFramework>
   ```
   - **Estimated Effort**: 5 minutes

3. **Enable Windows Desktop Support**
   ```xml
   <PropertyGroup>
     <OutputType>WinExe</OutputType>
     <TargetFramework>net10.0-windows</TargetFramework>
     <UseWindowsForms>true</UseWindowsForms>
   </PropertyGroup>
   ```
   - **Estimated Effort**: 5 minutes

4. **Update Package References**
   - Remove: System.Runtime.InteropServices.WindowsRuntime
   - Keep others initially for testing
   - **Estimated Effort**: 15 minutes

5. **Remove Obsolete Elements**
   - Old framework references (System, System.Core, etc.) - auto-referenced in SDK-style
   - BootstrapperPackage elements
   - Old MSBuild imports
   - **Estimated Effort**: 30 minutes

---

## Detailed Migration Steps

### Phase 0: Pre-Migration Preparation ✅

**Status**: COMPLETE

- [x] Create upgrade branch (`upgrade-to-NET10`)
- [x] Commit all pending changes
- [x] Generate assessment report

**Remaining Tasks**:

1. **Validate .NET 10 SDK Installation**
   - Run: `dotnet --list-sdks`
   - Ensure .NET 10.0 SDK is installed
   - Install from: https://dotnet.microsoft.com/download/dotnet/10.0

2. **Backup Current Build**
   - Build current version
   - Archive Release build artifacts
   - Document current version number

3. **Review Deployment Process**
   - Document current ClickOnce deployment
   - Plan new deployment strategy
   - Identify deployment dependencies

**Estimated Time**: 2-4 hours

---

### Phase 1: Project File Conversion

#### Step 1.1: Convert Texture64.csproj to SDK-Style

**Risk**: Medium ⚠️  
**Estimated Time**: 1-2 hours

**Process**:

1. **Option A: Use .NET Upgrade Assistant (Recommended)**
   ```powershell
   dotnet tool install -g upgrade-assistant
   upgrade-assistant upgrade .\Texture64\Texture64.csproj --target-tfm-support lts
   ```

2. **Option B: Manual Conversion**
   
   Create new SDK-style project structure:

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <OutputType>WinExe</OutputType>
       <TargetFramework>net10.0-windows</TargetFramework>
       <UseWindowsForms>true</UseWindowsForms>
       <ApplicationIcon>flower.ico</ApplicationIcon>
       <RootNamespace>Texture64</RootNamespace>
       <AssemblyName>Texture64</AssemblyName>
     </PropertyGroup>

     <ItemGroup>
       <PackageReference Include="Microsoft.Windows.SDK.Contracts" Version="10.0.26100.7463" />
       <PackageReference Include="System.Runtime.WindowsRuntime" Version="4.6.0" />
       <PackageReference Include="System.Runtime.WindowsRuntime.UI.Xaml" Version="4.6.0" />
     </ItemGroup>

     <ItemGroup>
       <Content Include="flower.ico" />
     </ItemGroup>
   </Project>
   ```

   **Note**: SDK-style projects auto-include all .cs files, so explicit Compile items are removed.

**Validation**:
- [ ] Project loads in Visual Studio 2022+
- [ ] All source files are recognized
- [ ] Resources and embedded files are included
- [ ] Designer files load correctly

---

#### Step 1.2: Remove Unnecessary Packages

**Risk**: Low  
**Estimated Time**: 15 minutes

**Actions**:
1. Remove `System.Runtime.InteropServices.WindowsRuntime` package reference
2. Keep other WindowsRuntime packages initially (test removal later)

**Validation**:
- [ ] Project restores packages successfully
- [ ] No package conflict errors

---

### Phase 2: Initial Build and Compilation Fixes

#### Step 2.1: First Build Attempt

**Risk**: High ⚠️  
**Estimated Time**: 2-6 hours

**Process**:

1. **Clean and Restore**
   ```powershell
   dotnet clean
   dotnet restore
   ```

2. **Build Project**
   ```powershell
   dotnet build .\Texture64\Texture64.csproj
   ```

3. **Analyze Build Output**
   - Document all errors
   - Categorize by type (missing APIs, incompatible types, etc.)
   - Prioritize fixes

**Expected Error Categories**:

| Error Type | Likelihood | Resolution Approach |
|-----------|-----------|---------------------|
| Missing type references | Medium | Add using directives or package references |
| Property/method not found | Medium | Update to .NET 10 equivalent API |
| Type conversion errors | Low | Update casts or conversions |
| Designer-generated code | Low | Regenerate designer files if needed |
| Legacy controls | Low-Medium | Replace with modern equivalents |

---

#### Step 2.2: Fix Windows Forms Issues

**Risk**: Medium ⚠️  
**Estimated Time**: 2-4 hours

**Focus Files**:
- TestForm.cs / TestForm.Designer.cs
- ImageForm.cs / ImageForm.Designer.cs
- GraphicsViewer.cs / GraphicsViewer.Designer.cs
- AboutBox.cs / AboutBox.Designer.cs

**Common Fixes**:

1. **Update Control Declarations**
   - Most controls should work without changes
   - Check for any legacy control usage (StatusBar, ContextMenu, etc.)

2. **Designer File Issues**
   - Open each form in Visual Studio designer
   - Let Visual Studio regenerate designer code if needed
   - Save and rebuild

3. **Event Handler Compatibility**
   - Most event handlers work unchanged
   - Verify delegate signatures match

**Validation Steps**:
- [ ] All forms compile without errors
- [ ] All forms open in designer
- [ ] No designer warnings
- [ ] Event handlers wire correctly

---

#### Step 2.3: Fix System.Drawing Issues

**Risk**: Medium ⚠️  
**Estimated Time**: 1-2 hours

**Focus File**: N64Graphics.cs

**Process**:

1. **Add System.Drawing.Common if Needed**
   ```xml
   <PackageReference Include="System.Drawing.Common" Version="9.0.0" />
   ```
   Note: Check if this is needed after initial build; may be included with Windows Desktop support.

2. **Review Image Manipulation Code**
   - Check Bitmap creation and manipulation
   - Verify Image property accesses (Width, Height, etc.)
   - Update any deprecated graphics methods

3. **Common Issues to Address**:
   - `System.Drawing.Bitmap` constructor usage
   - `System.Drawing.Image` property access
   - `ContentAlignment` enum usage
   - Graphics context disposal patterns

**Validation**:
- [ ] N64Graphics.cs compiles without errors
- [ ] Image loading works correctly
- [ ] Graphics rendering functions properly

---

#### Step 2.4: Fix Configuration Issues

**Risk**: Low  
**Estimated Time**: 30 minutes - 1 hour

**Current Config**: App.config

**Process**:

1. **Test Current App.config**
   - .NET 10 supports App.config for desktop apps
   - No changes may be needed

2. **If Issues Arise**:
   - Add `System.Configuration.ConfigurationManager` package:
     ```xml
     <PackageReference Include="System.Configuration.ConfigurationManager" Version="9.0.0" />
     ```

3. **Optional: Modernize Configuration**
   - Consider migrating to appsettings.json
   - Use Microsoft.Extensions.Configuration
   - Only if time permits and adds value

**Validation**:
- [ ] Configuration values load correctly
- [ ] No runtime configuration errors

---

### Phase 3: Complete Compilation

#### Step 3.1: Address Remaining Errors

**Risk**: Medium  
**Estimated Time**: 1-3 hours

**Process**:

1. **Iterate Through Compiler Errors**
   - Fix errors one by one
   - Rebuild frequently
   - Document unusual issues

2. **Common Remaining Issues**:
   - Missing using directives
   - Namespace changes
   - API signature changes
   - Type conversion updates

3. **Resources**:
   - [.NET 10 Breaking Changes](https://learn.microsoft.com/dotnet/core/compatibility/10.0)
   - [Windows Forms Migration Guide](https://learn.microsoft.com/dotnet/desktop/winforms/migration/)
   - Stack Overflow for specific API changes

**Validation**:
- [ ] Zero compilation errors
- [ ] Zero compilation warnings (goal)
- [ ] All projects build successfully

---

#### Step 3.2: Clean Build Verification

**Risk**: Low  
**Estimated Time**: 15 minutes

**Process**:

```powershell
# Clean everything
dotnet clean
Remove-Item -Recurse -Force .\Texture64\bin, .\Texture64\obj

# Full rebuild
dotnet restore
dotnet build --configuration Release

# Verify output
Test-Path .\Texture64\bin\Release\net10.0-windows\Texture64.exe
```

**Validation**:
- [ ] Clean build succeeds
- [ ] Release build succeeds
- [ ] Executable is generated
- [ ] File size is reasonable

---

### Phase 4: Testing

#### Step 4.1: Smoke Testing

**Risk**: Low  
**Estimated Time**: 1-2 hours

**Test Cases**:

1. **Application Launch**
   - [ ] Application starts without errors
   - [ ] Main window displays correctly
   - [ ] All UI elements render properly
   - [ ] No immediate exceptions

2. **Basic Functionality**
   - [ ] Menu items work
   - [ ] Buttons respond
   - [ ] File dialogs open
   - [ ] About box displays

**Document Issues**:
- Any visual glitches
- Performance problems
- Exception messages
- Unexpected behavior

---

#### Step 4.2: Feature Testing

**Risk**: Medium ⚠️  
**Estimated Time**: 4-8 hours

**Core Features to Test**:

1. **File Operations**
   - [ ] Open N64 graphics file
   - [ ] Import images
   - [ ] Export images
   - [ ] Save modified graphics

2. **Graphics Viewing**
   - [ ] Image display correctly
   - [ ] Zoom in/out functions
   - [ ] Color picker works
   - [ ] Palette operations

3. **Graphics Manipulation**
   - [ ] N64 format conversion
   - [ ] Image transformations
   - [ ] Color palette editing
   - [ ] Copy/paste operations

4. **UI Controls**
   - [ ] All numeric up/down controls
   - [ ] Table layouts
   - [ ] Group boxes
   - [ ] Tool strips and menus
   - [ ] Context menus
   - [ ] Status indicators

**Test Data**:
- Use existing N64 graphics files
- Create test files for edge cases
- Test with various image formats

**Document**:
- Any feature that doesn't work
- Performance differences (better or worse)
- Visual differences from .NET Framework version

---

#### Step 4.3: Regression Testing

**Risk**: High ⚠️  
**Estimated Time**: 4-6 hours

**Process**:

1. **Side-by-Side Comparison**
   - Run .NET Framework 4.8 version
   - Run .NET 10 version
   - Compare outputs for same inputs

2. **Test Scenarios**:
   - Load complex N64 graphics
   - Perform multiple operations
   - Verify output files match byte-for-byte (if applicable)
   - Check performance metrics

3. **Regression Checklist**:
   - [ ] All previously working features still work
   - [ ] Output quality is identical
   - [ ] File formats are compatible
   - [ ] No data loss or corruption
   - [ ] UI behavior is consistent

**Acceptance Criteria**:
- No functional regressions
- Performance is equal or better
- User experience is maintained or improved

---

#### Step 4.4: Performance Testing

**Risk**: Low  
**Estimated Time**: 2 hours

**Metrics to Measure**:

1. **Startup Time**
   - Measure time to show main window
   - Compare with .NET Framework version

2. **Graphics Operations**
   - Time image loading
   - Time format conversions
   - Time export operations

3. **Memory Usage**
   - Monitor memory during typical operations
   - Check for memory leaks (open/close files multiple times)

4. **Responsiveness**
   - UI response time to clicks
   - Rendering speed for large images

**Expected Results**:
- .NET 10 should be equal or faster
- Memory usage may be slightly different
- Startup time should be competitive

**Document Any Issues**:
- Significant performance degradation
- Memory leaks
- UI lag or freezing

---

### Phase 5: Deployment Configuration

#### Step 5.1: Choose Deployment Strategy

**Risk**: Medium ⚠️  
**Estimated Time**: 2-4 hours

**Options**:

1. **Self-Contained Deployment**
   - Includes .NET 10 runtime
   - Larger download (~150MB+)
   - Users don't need .NET installed
   - Best for: Wide distribution, no dependencies

2. **Framework-Dependent Deployment**
   - Requires .NET 10 runtime installed
   - Smaller download (~1-2MB)
   - Users must install .NET 10
   - Best for: Controlled environments, technical users

3. **ClickOnce Deployment**
   - Still supported in .NET 10
   - Automatic updates
   - Browser-based installation
   - Best for: Internal distribution, frequent updates

4. **MSIX Package**
   - Modern Windows packaging
   - Microsoft Store compatible
   - Sandboxed execution
   - Best for: Store distribution, enterprise deployment

**Recommendation**: Start with Framework-Dependent for simplicity, evaluate others as needed.

---

#### Step 5.2: Configure Publish Settings

**Risk**: Low  
**Estimated Time**: 1-2 hours

**For Framework-Dependent Deployment**:

Add to Texture64.csproj:

```xml
<PropertyGroup>
  <PublishSingleFile>false</PublishSingleFile>
  <SelfContained>false</SelfContained>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <PublishReadyToRun>true</PublishReadyToRun>
</PropertyGroup>
```

**Publish Command**:
```powershell
dotnet publish -c Release -r win-x64 --self-contained false
```

**For Self-Contained Deployment**:

```xml
<PropertyGroup>
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <PublishReadyToRun>true</PublishReadyToRun>
  <PublishTrimmed>false</PublishTrimmed>
</PropertyGroup>
```

**Publish Command**:
```powershell
dotnet publish -c Release -r win-x64 --self-contained true
```

**Validation**:
- [ ] Publish succeeds without errors
- [ ] Output folder contains all necessary files
- [ ] Executable runs on clean machine
- [ ] All features work from published version

---

#### Step 5.3: Test Deployed Application

**Risk**: High ⚠️  
**Estimated Time**: 2-4 hours

**Test Environment**:
- Clean Windows 10/11 VM or physical machine
- No Visual Studio installed
- No .NET SDK installed
- For Framework-Dependent: Install .NET 10 Runtime only

**Test Cases**:
1. **Installation**
   - [ ] Deployment package copies correctly
   - [ ] Application launches
   - [ ] No missing DLL errors

2. **Functionality**
   - [ ] Run all critical test cases from Phase 4
   - [ ] Verify file associations work (if applicable)
   - [ ] Check resource loading (icons, images)

3. **Uninstallation** (if applicable)
   - [ ] Application removes cleanly
   - [ ] No orphaned files

**Document**:
- Installation instructions for end users
- System requirements
- Any deployment issues encountered

---

### Phase 6: Documentation and Finalization

#### Step 6.1: Update Documentation

**Risk**: Low  
**Estimated Time**: 2-3 hours

**Documents to Update**:

1. **README.md**
   - Update system requirements (.NET 10)
   - Update build instructions
   - Update deployment instructions
   - Note any breaking changes for users

2. **Build Instructions**
   ```markdown
   ## Building

   ### Prerequisites
   - .NET 10 SDK
   - Visual Studio 2022 17.10+ (for designer support)

   ### Build Commands
   ```powershell
   dotnet restore
   dotnet build -c Release
   ```

   ### Running
   ```powershell
   dotnet run --project Texture64\Texture64.csproj
   ```
   ```

3. **Deployment Guide**
   - Document chosen deployment strategy
   - Provide step-by-step deployment instructions
   - Include troubleshooting section

4. **Change Log**
   - Document migration to .NET 10
   - Note any feature changes
   - List known issues (if any)

---

#### Step 6.2: Create Release Notes

**Risk**: Low  
**Estimated Time**: 1 hour

**Release Notes Template**:

```markdown
# Texture64 v2.0 - .NET 10 Migration Release

## Major Changes

- Upgraded from .NET Framework 4.8 to .NET 10.0 LTS
- Modernized project structure to SDK-style format
- Updated deployment to [Framework-Dependent/Self-Contained]

## System Requirements

- Windows 10 version 1809 or later
- .NET 10.0 Runtime (download: https://dotnet.microsoft.com/download/dotnet/10.0)

## Known Issues

- [List any known issues or limitations]

## Migration Notes

- Settings and data files are compatible with previous version
- [Any user-facing changes]

## Technical Details

- Target Framework: net10.0-windows
- Compiled with .NET 10.0 SDK
- [Other technical notes]
```

---

#### Step 6.3: Commit and Merge

**Risk**: Low  
**Estimated Time**: 30 minutes

**Process**:

1. **Final Review**
   - Review all changes
   - Ensure no debugging code remains
   - Check for any TODO comments

2. **Commit Changes**
   ```powershell
   git add -A
   git commit -m "Upgrade to .NET 10.0

   - Convert project to SDK-style format
   - Update target framework to net10.0-windows
   - Remove unnecessary package dependencies
   - Update documentation and build instructions
   - Test all functionality
   
   Migration completed successfully with full feature parity."
   ```

3. **Create Pull Request**
   - Create PR from `upgrade-to-NET10` to `master`
   - Include summary of changes
   - Reference assessment and plan documents
   - Request review if applicable

4. **Merge Strategy**
   - Recommended: Squash and merge for clean history
   - Alternative: Merge commit to preserve full migration history

---

## Risk Management

### High Risk Items

| Risk | Impact | Probability | Mitigation | Contingency |
|------|--------|-------------|------------|-------------|
| API breaking changes block compilation | High | Medium | Use upgrade assistant, consult docs | Seek community help, consider staying on .NET 8 temporarily |
| Designer files corrupted during conversion | High | Low | Backup before conversion, version control | Manually recreate forms from backup |
| Third-party package incompatibility | High | Low | All packages are compatible per assessment | Find alternative packages or remove features |
| Graphics rendering broken | High | Low | Extensive testing, side-by-side comparison | Fix rendering code, use compatibility layer |
| Performance regression | Medium | Low | Performance testing, profiling | Optimize hot paths, consider different build options |

### Medium Risk Items

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Deployment configuration issues | Medium | Medium | Test on clean machines |
| Legacy controls need replacement | Medium | Low | Search codebase proactively |
| Configuration system changes | Medium | Low | Test configuration loading early |
| UI visual differences | Medium | Low | Visual testing, user feedback |

### Rollback Plan

**If Migration Fails**:

1. **Immediate Rollback**
   ```powershell
   git checkout master
   git branch -D upgrade-to-NET10
   ```

2. **Preserve Work**
   ```powershell
   git checkout upgrade-to-NET10
   git tag migration-attempt-1
   git checkout master
   ```

3. **Reassess**
   - Review what failed
   - Update plan with lessons learned
   - Consider alternative approaches
   - Re-attempt when issues resolved

---

## Success Criteria

The migration is **complete and successful** when:

### Technical Criteria ✅

- [ ] Project targets net10.0-windows
- [ ] Project uses SDK-style format
- [ ] Solution builds with zero errors
- [ ] Solution builds with zero warnings (stretch goal)
- [ ] Unnecessary packages removed
- [ ] Clean build succeeds

### Functional Criteria ✅

- [ ] All forms load and display correctly
- [ ] All features work as before
- [ ] File operations (open/save/export) work
- [ ] N64 graphics conversion functions correctly
- [ ] No data corruption or loss
- [ ] Performance is equal or better

### Quality Criteria ✅

- [ ] No regressions from .NET Framework version
- [ ] Code quality maintained
- [ ] No new bugs introduced
- [ ] User experience preserved

### Deployment Criteria ✅

- [ ] Application deploys successfully
- [ ] Runs on target machines
- [ ] Installation documented
- [ ] Deployment tested on clean machine

### Documentation Criteria ✅

- [ ] README updated
- [ ] Build instructions updated
- [ ] Deployment guide created
- [ ] Release notes written
- [ ] Known issues documented

---

## Checklist Summary

### Pre-Migration ✅
- [ ] .NET 10 SDK installed and verified
- [ ] Current build backed up
- [ ] Deployment process documented
- [ ] All prerequisites met

### Migration Execution
- [ ] Project converted to SDK-style
- [ ] Target framework updated to net10.0-windows
- [ ] Unnecessary packages removed
- [ ] Solution builds successfully
- [ ] All compilation errors resolved

### Testing
- [ ] Smoke tests passed
- [ ] Feature tests passed
- [ ] Regression tests passed
- [ ] Performance tests passed
- [ ] Deployment tests passed

### Finalization
- [ ] Documentation updated
- [ ] Release notes created
- [ ] Changes committed
- [ ] Pull request created
- [ ] Code reviewed (if applicable)
- [ ] Merged to main branch

---

## Appendix A: Quick Reference Commands

### Build Commands
```powershell
# Restore packages
dotnet restore

# Build Debug
dotnet build

# Build Release
dotnet build -c Release

# Clean
dotnet clean

# Run
dotnet run --project Texture64\Texture64.csproj
```

### Publish Commands
```powershell
# Framework-dependent
dotnet publish -c Release -r win-x64 --self-contained false

# Self-contained
dotnet publish -c Release -r win-x64 --self-contained true

# Single-file
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### Git Commands
```powershell
# Check status
git status

# Stage changes
git add -A

# Commit
git commit -m "Your message"

# View branch
git branch

# Switch branch
git checkout master

# Create PR (GitHub CLI)
gh pr create --base master --head upgrade-to-NET10 --title ".NET 10 Upgrade"
```

---

## Appendix B: Troubleshooting Guide

### Common Issues and Solutions

**Issue**: "The SDK 'Microsoft.NET.Sdk.WindowsDesktop' specified could not be found"  
**Solution**: Update to .NET SDK that includes Windows Desktop workload, or use `Microsoft.NET.Sdk` with `<UseWindowsForms>true</UseWindowsForms>`

**Issue**: "CS0234: The type or namespace name 'Forms' does not exist in the namespace 'System.Windows'"  
**Solution**: Add `<UseWindowsForms>true</UseWindowsForms>` to PropertyGroup

**Issue**: Designer won't load forms  
**Solution**: Ensure Visual Studio 2022 17.10+, try rebuilding, close and reopen designer

**Issue**: "System.Drawing.Common is not supported on this platform"  
**Solution**: Ensure targeting `net10.0-windows`, not just `net10.0`

**Issue**: Missing assemblies at runtime  
**Solution**: Use `dotnet publish` instead of `dotnet build` for deployment

**Issue**: App won't start on deployment machine  
**Solution**: Ensure .NET 10 Runtime installed, check for missing dependencies with Dependency Walker or similar

---

## Appendix C: Resource Links

### Official Documentation
- [.NET 10 Documentation](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10)
- [Windows Forms Migration Guide](https://learn.microsoft.com/dotnet/desktop/winforms/migration/)
- [SDK-Style Projects](https://learn.microsoft.com/visualstudio/msbuild/how-to-use-project-sdk)
- [.NET 10 Breaking Changes](https://learn.microsoft.com/dotnet/core/compatibility/10.0)

### Tools
- [.NET Upgrade Assistant](https://dotnet.microsoft.com/platform/upgrade-assistant)
- [.NET Portability Analyzer](https://github.com/microsoft/dotnet-apiport)
- [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Community Resources
- [Stack Overflow - .NET Migration Questions](https://stackoverflow.com/questions/tagged/.net-core+migration)
- [GitHub - Upgrade Assistant Issues](https://github.com/dotnet/upgrade-assistant/issues)

---

**Plan Status**: Ready for Execution  
**Next Step**: Begin Phase 0 - Pre-Migration Preparation

---

*This plan was generated by GitHub Copilot App Modernization Agent based on the assessment findings.*
