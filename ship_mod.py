import os
import shutil
from pathlib import Path

def copy_directory(source_dir: str, destination_dir: str):
    source_path = Path(source_dir)
    destination_path = Path(destination_dir)
        
    destination_path.mkdir(parents=True, exist_ok=True)
        
    for item in source_path.iterdir():
        destination_filepath = destination_path / item.name
        if item.is_dir():
            if destination_filepath.exists():
                shutil.rmtree(destination_filepath)
            shutil.copytree(item, destination_filepath)
        else:
            shutil.copy2(item, destination_filepath)
            
    print(f"Directory Copied: {config_dir}")

if __name__ == "__main__":  
    mod_dir = "Ursur"
    config_dir = f"{mod_dir}Configs"    
    content_destination_dir = mod_dir
    
    script_dir = os.path.dirname(os.path.abspath(__file__))
    
    source = f"{script_dir}/{config_dir}"
    bepinex_dir = os.path.abspath(os.path.join(script_dir, '..', '..', '..'))
    destination = f"{bepinex_dir}/config/Obeliskial_importing/{content_destination_dir}"
    
    copy_directory(source, destination)

    mod_destination = f"{script_dir}/{mod_dir}/BepInEx/config/Obeliskial_importing/{content_destination_dir}"
    copy_directory(source, mod_destination)

    output_name = f"{script_dir}/{mod_dir}"
    zip_dir = f"{script_dir}/{mod_dir}"
    shutil.make_archive(output_name, 'zip', zip_dir)